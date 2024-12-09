
## 光源推定結果

### テスト動画
![スクリーンショット 2024-12-08 174258](https://github.com/user-attachments/assets/91687edc-0149-4362-9843-5175be012351)

動画
https://www.youtube.com/shorts/Rb0cueQHbfk

動かしている照明に対応してリアルタイムに影が出ている

### 計算結果の画像表示

<img src="https://github.com/user-attachments/assets/16df062e-9c6f-45d9-a8be-e5984a7fdc39" width="300">

左上）光源位置ピクセルを赤くする　（１ピクセルが真ん中のほうに映っている）　左下）光源以外をマスクしたもの　右下）入力360度画像

### 計算結果のレンダリング
<img src="https://github.com/user-attachments/assets/7f534ce8-f12e-482f-ab21-4bc95b8cd9d5" width="300">

光源推定で算出した、光源の方向、強さの結果をUnityのディレクショナルライトに適用した結果。右下の黒と赤の正方形は、黒を原点としたときの光源の方向を赤の正方形で表しており、赤の正方形の方向からオブジェクトが照らされる。




## 光源推定コード
### CPURunEstimation.cs

入力360度画像から光源の方向、強さ、色を計算する


### CPURunEstimation.compute

入力画像をcomputeshader を使用しHDR画像に変換する

### ワークフロー
1. 360度カメラからの入力をobsに表示する。

2. 表示したものをndi通信でunity内のRenderTextureに反映。

3. このRenderTextureをCPURunEstimation.csの入力として受け取る

4. 光源推定Estimation()でRenderTextureを入力とし、出力として光源の方向、強さ、色を出す。

5. 得られた光源情報をUnity上のDirectionalLightに適用し、オブジェクトのライティングができる。

6. 以下、１～５の繰り返し

   光源推定関数Estimationは先行研究[1]をそのままC#で書いている

# 光源推定先行研究
[1] Taehyun Rhee, Member, IEEE, Lohit Petikam, Benjamin Allen, and Andrew Chalmers,MR360: Mixed Reality Rendering for 360° Panoramic Videos,IEEE TRANSACTIONS ON VISUALIZATION AND COMPUTER GRAPHICS, VOL. 23, NO. 4, APRIL 2017 

