import os
import argparse
import numpy as np
from PIL import Image
from scipy.interpolate import RegularGridInterpolator

import matplotlib.pyplot as plt
import torch

from slice_model import SliceNet
from misc import tools, eval

# 深度推定の入力画像
def_img = 'example/001ad2ad14234e06b2d996d71bb96fc4_color.png'

def_gt = 'example/001ad2ad14234e06b2d996d71bb96fc4_depth.png'

# 深度推定モデルの重みファイル
def_pth = 'ckpt/resnet50_m3d.pth'


if __name__ == '__main__':
    # 事前パラメータ
    parser = argparse.ArgumentParser(formatter_class=argparse.ArgumentDefaultsHelpFormatter)
    parser.add_argument('--pth', required=False, default=def_pth,
                        help='path to load saved checkpoint.')
    parser.add_argument('--img_glob', required=False, default=def_img)
    parser.add_argument('--gt_depth', required=False, default=def_gt)
    parser.add_argument('--no_cuda', action='store_true', default=False)
    args = parser.parse_args()


    # モデルのロード
    device = torch.device('cpu' if args.no_cuda else 'cuda')
    net = tools.load_trained_model(SliceNet, args.pth).to(device)
    net.eval()

    # 推論  
    # 入力の作成
    img_pil = Image.open(args.img_glob)
    full_W, full_H = img_pil.size
    H, W = 512, 1024
    img_pil = img_pil.resize((W, H), Image.BICUBIC)
    img = np.array(img_pil, np.float32)[..., :3] / 255.
    #　モデルに入力を通し、デバック
    with torch.no_grad():
        #### predict depth
        x_img = torch.FloatTensor(img.transpose([2, 0, 1]).copy())       
        x = x_img.unsqueeze(0)
        depth = net(x.to(device))  
        print("export start")

        # モデルをOnnxにエクスポート
        torch.onnx.export(net,               # model being run
                        x.to(device),                         # model input (or a tuple for multiple inputs)
                        "super_resolution.onnx",   # where to save the model (can be a file or file-like object)
                        export_params=True,        # store the trained parameter weights inside the model file
                        opset_version=15,          # the ONNX version to export the model to
                        do_constant_folding=True,  # whether to execute constant folding for optimization
                        input_names = ['input'],   # the model's input names
                        output_names = ['output'], # the model's output names
                        dynamic_axes={'input' : {0 : 'batch_size'},    # variable length axes
                                        'output' : {0 : 'batch_size'}})
        print("export end")

        #　モデル出力結果を表示
        plt.imshow(depth.cpu().numpy().squeeze())  
        plt.show() 
        print("depth: {}".format(depth.shape))
