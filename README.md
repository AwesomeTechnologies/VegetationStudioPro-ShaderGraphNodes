# Vegetation Studio Pro- Shader Graph Nodes

This repository contains custom nodes for use with the Unity Shader Graph. 
They will help you add Vegetation Studio support for instanced indirect rendering, touch react and other custom features. 

The nodes should work on both LW and HD SRP from version 4.2. 

Nodes will be added when available. 

If you include these nodes with a Unity Asset make sure the original .meta file is included. This way there will not be problems with multiple assets including the same file. Make sure your file is using the latest version from this repository before updating. 
Also include a reference to the MIT licence of the node files. 

<h3><b>Instanced indirect node</b></h3>
Vegetation Studio Pro supports instanced indirect rendering. In order for shaders to use this they need some additional setup.
Adding this node to a shader graph will do that for you automatic. No need for additional include files or manual edit of the shader source. 

Add it as a normal node from the menu. Select the correct node for your SRP (LW or HD). There is some difference in implementation since HD SRP renders with positions relative to camera position to remove float resolution artifacts when far away from 0,0,0

<img src="https://www.awesometech.no/wp-content/uploads/2018/11/Image-860.png"/>

In order for the node to evaluate and inject the functions and #pragmas needed for Vegetation Studio Pros instanced indirect support it needs to be part of the shader graph. It is dessigned as a color passthrough node. 

In order for the node to evaluate on both normal and shadow passes it needs to be connected to the position socket on the master node. Give it an position in object space as input as in the image below. 

<img src="https://www.awesometech.no/wp-content/uploads/2018/11/Image-861.png"/>


