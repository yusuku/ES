## 深度推定の結果
![スクリーンショット 2024-12-08 164317](https://github.com/user-attachments/assets/6fb438fb-2a15-4f67-8373-9a09c3d64ff9)

左）深度推定画像の各ピクセルの値を参照し3D位置を計算しUnity上で、GPUInstancing を使用し計算した３D位置にキューブを配置した

右上）深度推定[1] 画像。暗いピクセルはカメラから遠くに、明るいピクセルはカメラの近くにあることを示している。

右下）入力360度画像

## コード
#### SliceNetMInference.py
深度推定モデルSliceNetをOnnxファイルにエクスポートする

#### Slicenet2MaskOcclusion.cs
SliceNetのOnnxファイルをインポートし、入力360度画像から深度推定画像を出力する


## 深度推定モデル

[1]SliceNet https://github.com/crs4/SliceNet
