
## 光源推定結果

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
360度カメラからの入力をobsに表示する。

表示したものをndi通信でunity内のRenderTextureに反映。

このRenderTextureをCPURunEstimation.csの入力として受け取る

#### CPURunEstimation.csの光源推定Estimation()ワークフロー

Estimation()が入力画像LDRtexを使用し、閾値処理により光源ピクセルを決定し、光源の極座標polar、放射輝度Elを出力する

##### Inverse Tone Mapping
入力RenderTextureをCPURunEstimation.computeを使用してHDR画像に変える。

##### Thresholding approach
全ピクセルの輝度を計算し、全ピクセル輝度の平均、分散を計算する。

輝度の閾値=平均+2*分散

として、閾値を決定し、以降この値以上の輝度を持つピクセルを光源のピクセルとする

##### Breath first Search
幅優先探索で輝度の閾値以上のピクセルの連結成分を抽出し、各連結成分をそれぞれ異なる一つの光源からくるものとする。

##### Pixel Irradiance,Lights irradiance
入力画像がEquirectangular projectionで　球形の360度画像を2D画像に投影していることを考慮し、各ピクセルの立体角を計算し、
##### Light's position



EstiVales estivlaues = new　EstiVales(PolarPosion, Els);


