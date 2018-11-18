# Vegetation-Studio-Pro---Shader-Graph-Nodes

This repository contains custom nodes for use with the Unity Shader Graph. 
They will help you add Vegetation Studio support for instanced indirect rendering, touch react and other custom features. 

The nodes should work on both LW and HD SRP from version 4.2. 

Nodes will be added when available. 

<h3><b>Instanced indirect node</b></h3>
Vegetation Studio Pro supports instanced indirect rendering. In order for shaders to use this they need some additional setup.
Adding this node to a shader graph will do that for you automatic. No need for additional include files or manual edit of the shader source. 

Add it as a normal node from the menu. 

<img src="https://www.awesometech.no/wp-content/uploads/2018/11/Image-860.png"/>

In order for the node to evaluate and inject the functions and #pragmas needed for Vegetation Studio Pros instanced indirect support it needs to be part of the shader graph. It is dessigned as a color passthrough node. Connect it between your texture or color parameter and the master node. It will not change the value only inject the extra code. 

<img src="https://www.awesometech.no/wp-content/uploads/2018/11/Image-859.png"/>


