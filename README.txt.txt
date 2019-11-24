Wesley Johnson; C00157698
Dr. Arun Kulshresth
CMPS 327-001
11/23/19

CMPS 327 Project 5 Bugs, Errors, and Report

Project 5 does not work properly. The project has not changed drastically from its template state, and it contains multiple errors. The A* algorithm is not properly implemented into the project, and the only movements available in the project are enemy movements.

One enemy moves in a random area while the other is offscreen and slowly moves towards the maze. The random enemy movement was a premade behavior in the project, but the enemy moving outside the project comes from the implementation of EnemyBehavior2. The enemy moving outside the maze is labeled as utilizing EnemyBehavior1, but it moves outside the maze because it detected the player object and quickly moved away from the player object. The randomly moving enemy will do the same thing if the player object is manually moved towards the enemy or vice versa. These behaviors show that the enemy behavior finite state machine registers the player object and works to a certain degree, but the enemy's behavior code needs adjustment. The current code is derived from the Pursuit algorithm from the previous project, but it does not hold the previous project's standards.

The A* algorithm does not work and is not complete in its implementation in the class "Pathfinding.cs." A seperate file with a more organized A* algorithm from a series of YouTube videos is in the project, but the better organized algorithm does not work either. Links to the videos will be provided below and are in "Pathfinder.cs" and the folder "A_Star_Video." The better algorithm and scripts were built in a separate project and were tested there. Their inclusion is to demonstrate a potential better implementation of the A* algorithm.

The main issue I had with the project was transferring what I learned and wrote in the alternate A* Algorithm scripts into the main scripts. The template requires the programmer to find a way to work with the class "Tile" in "Tile.cs" and the class "Node" in "PathFinder.cs." The unintuitive separation and combination of the classes made it difficult to implement the A* algorithm found online and difficult to access and a working algorithm with the correct data types.

I know the work for this project was subpar. The final project will, hopefully, be much better.

GitHub Link:
https://github.com/R3S3ARCH3R-6/C00157698_CMPS_327_Project_5.git

YouTube Video/Source Links:
https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s
https://www.youtube.com/watch?v=mZfyt03LDH4&t=8s (coding the actual A* alg)