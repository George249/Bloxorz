
Please check this out 
https://github.com/George249/Bloxorz/blob/master/GameProject.mp4


**Introduction:**

Bloxorz is a 3D puzzle game  The objective of the game is to get the block to fall into a square hole that is at the end of each stage, 

To reach the end you have to take smart and careful movements.

there are 8 stages to complete, in each stage there is a menu button , which takes us to the main scene.

The player will play as a block and will use the arrow keys to complete the objective of the game.

**The special features of the game :** 

1.Lives - in each stage the player has 3 lives , if he loses all of them , then he goes back two levels back.

Except for the first and second stages, in the first stage he starts the same one again, and in the second stage he goes back to the first one.

2.Orange spot  - when the player(block) stays on the orange block for more than 2 seconds, it explodes.

3.Moving Cube - when the player touches the moving cube ,the game is over.

4.Bridge - connects two parts of the floor ,there is a special tile that needs to be pressed(by the player) and the bridge will open.

**A special add up in this game is using platonic solid shapes instead of the cube for the player.(Tetrahedron,Dodecahedron,Icosahedron,Octahedron)**


**Description of project:**

the code consists of the scripts below:

**1.GAME MANAGER :** 

This script is responsible for tracking over the game and set what is going next after  some events.

` `Functions:

`                  `LevelCompleted-  calls the loadNextLevel  function after 1 sec.

`                  `loadNextLevel - load the next scene (buildIndex + 1)**.**

`                  `EndGame - calls the function that is responsible for updating the lives text.

`		`it either ends the game or calls **Restart**.

`	      `Restart- load the same scene again.





**2.PLAYER :** 

This script is responsible for the player movement(cube) - 

` `Functions:

`          `- UPDATE function - we check which arrow is pressed on the keyboard, so we can know to which side to rotate the cube , and we rotate 90 degrees.

` `this function also checks if the player reached the winspot or not ,  if yes then the LevelCompleted func' is called from the game manager,

and it invokes the load next scene function.

`           `-Fixed\_Update Function -  which is called every fixed frame-rate frame, we use the Time.fixedDeltaTime + the rotation time, to help know how  many times to be called.

We calculate the distance from the start position and create a new vector by adding those x,y,z distances to the start position and assign it to the transform position.

` `As for the rotation we use Quaternion Lerp (Quaternions are used to represent rotations, 

while Lerp Interpolates between two numbers which in our case is preRotation and postRotation and normalizes the result afterwards.



`           `-OnCollisionEnter - is called when this collider/rigidbody has begun touching another rigidbody/collider.(that's why we attach a rigid body to the player and the ground cubes.

` `we change the isGrounded to true if the player hits the ground, and checks when the player hits the death frame, if it does then we call the function to decrease the lives parameter in this level.

`           `-SetRadius - This function allows the motion of the block to work correctly when changing the rotation and transform of the block. it sets the radius of the blocks center to the 4 pivot points,  by checking when the direction of the block is in the same direction of global.(x,y,z)

**3.LEVEL\_CONTROLLER** : Loads the next scene , by their buildIndex number. 

**4.SPOT\_EXPLODE** : 

Variables :

` `-cubeSize,cubeInRow,cubesPivotDistance,cubesPivot - so we can explode the tile cube to tiny cubes.

\- explosionForce,explosionRadius,explosionUpward - so we can decide the explosion size.

` `Functions:

`                   `update - checks if the player stands on the orange spot, if so we Invoke function “explode” 2 seconds later.

`                  `explode - checks if the player is still on the orange spot, if so we proceed to the explosion part, by using the addExplosionForce  ,which applies a force to a rigidbody that simulates explosion effects(using the variable above).

`                  `createPiece - creates little pieces of the cube , and we use it in the explode function.


**5.BRIDGE\_CLICK** : contains a game object variable called followBridge.

`                 `We attach this script to the spot with the bridge click , and create tiles as a follow bridge sets their y transform to -50 , and assign it to the follow bridge variable.

`                 `Functions:

` `fixed\_update checks if the player is on the bridge click , if so , the “follow bridge” will open(the y transform is now 0).

**6.LIVES** : creates a textmesh of "lives:"

`          `we use it to update the lives , when the player dies by using the HelpUI script.

**7.HELPUI:** Functions: 

handleLoadingPreviousScene function - checks in which scene the player is, so we can know to which scene to go if the player loses.

`                     `UpdateLivesNumber - checks if lives is < 0 , then Invoke the "handleLoadingPreviousScene,  else , we update the lives, using the updateThelives function in lives script.

**8.MAIN\_SCRIPT\_MENU:** Attached to the menu scene , has two functions for starts and quit.

`                    `start is connected to the start button and open level 1. 

`                    `quit is connected to the quit button.



**9.MOVING\_CUBE :** 

Contains a cube that is moving on the floor, the player loses when touching this cube.

Variables :

\- the object that make trigger on touching (player)

\- directions for the cube to move.

\- array of points (as is explained in start function)

\- index for keep tracking over  the next index for the points array.

` `Functions:

`                  `Start - calculate 4 points in the direction and initialize the array with 

these points  .

`                  `Update  - take the next target pont from the array and index,

`		`And make the cube move towards this point. Update the index +1 % 4.

`                  `OnTriggerEnter- calls the endgame function when the player touches the

`		`cube.





**10.PLAYER\_SOLIDS** : This script is responsible for the player movement(with the other platonic solids)  -We created the shapes using blender , and for each shape we created a base that is built with the same shape . Functions: 

`           `GetRotation :

We find the “target tile “ , the one that the shape will stand on when it rotates, by function GetNearestTile - it gets the nearest tile from the ground that is attached to the variable transform ground.

We need to align the player to the target tile , because these shapes are not symmetric like the cube(and would possibly not be straight after rotation ), so we align one of the vertices of the player mesh  to the base mesh (which is from the same shape) - by getting the highest/closest vertices,  then we adjust the target position so it lines up with the player.

At last we rotate the player so at least one edge aligns with the base mesh , by getting the neighbours of the target tile. 



**THE UNITY PART:**

` `- One scene for the menu - contains Play and Quit buttons.

` `- 8 scenes for the original game  :

`      `1. Level 1 contains the moving cube feature.

`      `2. Level 2 - 4 regular.

`      `3. Level 5-7 contains the bridge feature.

`      `4. Level 8 contains the orange spot feature.

`   `The rest of the levels contain the platonic solids shapes instead of the cube.

Each scene has a menu button , which takes us to the main scene when it's pressed.

` `-Blender files for Tetrahedron,Dodecahedron,Icosahedron,Octahedron.

` `-Blender files for the platonic solids base(floor tiles).

We created prefabs for all types of tiles: (for the original game with the block)

`   `1.Player

`   `2.Moving Cube

`   `3.Winspot 

`   `4.TimerGround (for the orange spot )

`   `5.BridgeClick 

`   `6.floor (regular tile )

Packages:

`   `1.Pro-Builder - it helps with building and moving the objects in a constant distance






**To Sum Up :** 

We have really enjoyed working on this project despite of all the defficults,

This was for both of us the first time working on unity and with c# programming language, 

We have failed many times and faced a lot of problems, and eventually we have succeeded to get through them.

The part of calculating the movement for platonic solids shapes was really frustrating, but after many tries in the end we have got pretty good results.

Adding the feature was really fun, every feature has its own challenging part, we got so happy and excited seeing each feature works.

This experience has opened our eyes on the world of Game programming.

We have developed our skills ,learned many different new things that we found so interesting, and this is what kept us eager to continue and reach the goal.


