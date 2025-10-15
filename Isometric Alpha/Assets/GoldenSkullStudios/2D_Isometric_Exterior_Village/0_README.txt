2D-Isometric-Tile-Village-Pack by Golden Skull

*****************************************************************
***Update History***
*****************************************************************
Initial Release: March 2017

*****************************************************************
***Message to our dear supporters***
*****************************************************************
Thank you so much for purchasing the 2D Isometric Village Pack!
I really hope you will be able to put it to good use and use it in games, prototypes and whatever else you feel like creating with it.
Your support helps me to pay my expenses table while I work hard to create more game art packages. If you would like to support my work even more, please consider paying more than the average, so I can improve existing packages at a faster rate.

*****************************************************************
***Message to pirates***
*****************************************************************
If you have not purchased this package, please consider supporting the developer of this package, who spent a lot of time and effort with the creation of the package. Developing assets does not make you rich and the prices are a bargain compared to the amount of time that went into making them.
Also, please keep in mind that you are using copyrighted material and without a license key, you won't be able to use the tileset in any public projects.

*****************************************************************
***Technical Details***
*****************************************************************
-Each Tile is set up within a 512px x 512px grid cell.
-The basic isometric grid is set up by 2:1 proportions and not by angle.
-The base grid is x:400px y:200px

*****************************************************************
***Setup in Tiled***
*****************************************************************
-Map with Tile Size: Width: 400px and Height: 200px
-Tileset with Tile: width: 512px and height: 512px
-Tileset Drawing Offset X: -56px, Y: 0px ==> (-56,0)

*****************************************************************
***Unity 2D-Grid Setup***
*****************************************************************
-Pixels Per Unit: 100
-There will be an issue with stacking(height) as the tiles are optimized for a 3D-grid setup(see below).

*****************************************************************
***Unity 3D-Grid Setup***
*****************************************************************
-Spritesheet Import options: Pixels Per Unit: 283
-Create Empty Object at (0,0,0)
-Add your Sprite at (0,0,0)
-Rotate Sprite by (30,45,0)
-To align Sceneview: Select rotated Sprite, set SceneView to orthographic, click GameObject -> AlignViewToSelected
-To align Camera: Select Camera, set Projection to Orthographic, click GamObject -> AlignWithView

*Advantages of the 3D-Grid Setup*
-Maps built with this setup can support 3d-tiles as well as 2d tiles.
-You can simply use the grid to place your tiles, without worrying about the sorting.
-If you add Box-Colliders to the tiles(1 unit in size & centered), you can simply setup collisions with a 2d-world

*Disadvantages*
-not pixel perfect (if you look VERY closely at 1000% zoom, you can see pixels not overlapping fully)
-sorting-algorithms still needed when using a seamlessly moving character (we have not found an ideal solution how to achieve proper sorting with this setup yet, but we are looking into it.)

*****************************************************************
***Support***
*****************************************************************
If you run into issues with this package or have update requests, please join our Discord server for support:
https://discord.gg/SvShnpG

We will thoroughly evaluate your feedback and update our pack accordingly to fit your needs.