## GameAIFinal - MDP Tower Defence AI<br />

Collaboration with:
Igor Carvalho 
Daniel Hendricks
Jeremy Torella

In this Unity game we built a tower defense game where there are two types of towers and minion: air and ground.<br />
Air towers only attack air minions and same for ground towers and ground minions. <br />
The AI checks to see if there a minions on the board, if so it will place a tower near its location to try and kill it. Placing a tower costs 100 gold. Should it decide to place a tower it will prioritize location where it can posibly kill multiple enemies. <br />

On the gif below, at the start the AI places an air tower where it could posibly kill two air minions. Then it starts placing towers to kill the minions. <br />
Towards the end of the gif there is still a ground minion left but there is no location to place it thus the AI still places a ground tower on the board. <br />


<img src="Tower Defense.gif" width="800" height="450"> 

Future improvements include:
- Placing towers more towards the base which is on the top left.
- Prioritizing location more near the base
- The minions have a 10% chance of going left, 80% chance going towards the base, and 10% chance going right. Use this information to place towers.
