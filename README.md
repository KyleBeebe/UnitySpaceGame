# UnitySpaceGame

This was the result of a month long collaborative project for CMSC 425 (Game Programming) at UMD. 
The purpose of the game was prototyping interesting video mechanics and systems for implementation in Unity3D.

<a href="Space Force/Assets/Scripts/Enviro">Scripts</a> 

Space Force
Team: 
Kyle Beebe, Jason Ramsey, Philip Gouldman.


Description:
 </br>
 <p>
 
 Controls
 ---------
 
- P :  pause 
- wasd : movement 
- q or esc : exit
- left mouse button : shoot gun
- right mouse button :
    (click once : shoot grapple)
    (hold : grapple to point) 
- space bar : jump

You have a laser pistol on your left click and grapple on your right. You have a health bar in the top left. Death restarts you back at the clone chamber. Click the grapple once to attach it to a wall, your movement is restricted while tethered to geometry. If you hold right click while attached to something, your grapple reels and pulls you toward the point where your grapple is bound to. There are enemy robots in each area of the game. The sentries are bound to walls and will track you with their lasers, dealing damage to you for while the lasers touch you. There are flying robots and rolling robots. Shoot them to kill them. They will explode on death. There are two end rooms from the two branching directions at the start. One will bring you to a greenhouse cylinder area where you can break the glass on the gardens and set the plants on fire with your gun. The other end area is a core room with the rolling robots. All of the wall tiles here are deformable if you shoot them enough.
 </p>
 <p>
 All of the assets for the sentry were created in blender and Probuilder, along with all of the level geometry. The space skybox was generated using a space background generator application called “Universe”. The planet with the rotating atmosphere seen from the greenhouse window was created with the texture being pulled from free planet photos from NASA. Every script was written by us. The water spray particle effect in the greenhouse, and the steam and spark particle effects were created by us. We created the deformable tiles in blender using blend shapes and the opening doors, rotating rings, and mono-rail using keyframes. The grapple uses four control points with rigid bodies. Originally, it used a strung together line of cylinder objects. It looked more impressive but was a pain to work with. Now it is based on a Bezier curve line renderer. The four control points represent the four points for a cubic Bezier curve. 50 points spaced between them are evaluated on the curve and fed into a line render to render a bunch or smaller lines to create the rope. The rigid bodies are not constrained to a max distance from the line, so if you stand still and wave the grapple from side to side, the configurable joints between the points will break and cause the line to freak out. The areas have been collision boxed off pretty well; however, the force applied from grappling may put the character through a wall if traveling fast enough. Lastly, the cylinder greenhouse area rotations to keep the player oriented could be handled better. Ideally, the rotation would have been handled in Update() but had to be stuck in FixedUpdate() due to the grapple having rigid body physics and also needing to be in FixedUpdate(). The grapple line had a tendency to double-render but the camera was smooth, now the camera is a bit jittery in that area but not too bad. Lastly, pressing esc or Q to go to the main menu works in the editor but as a stand alone exe. It closes the application. 
 </p>


Video Link:
https://www.youtube.com/watch?v=S0BBw31RJLE&t=6s

