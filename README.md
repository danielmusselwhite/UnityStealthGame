# Unity Stealth Game

## Player:

## Enemy:

### Basic Seeker:

#### Pathfinding
* TODO : A* Algorithm

##### Graph Building
* Currently a simple equal sized grid map
* TODO:
   * Draw a ray in each direction until we hit a wall
   * Then draw a box of that size
   * At each wall, check the angle of rotation:
       * If angle of rotation is under a threshold, start drawing rays from there at that angle to explore if we move up
           * If ray is no longer under the ramp; that's the end of the box and we'll do normal graphbuilding from this level
       * If angle of rotation is over a threshold, check the threshold height difference for this (eg stairs); 
           * if it is under the threshold consider it as a ramp anyway
           * if it is over the threshold then this is an impassable wall

#### FSM

* 3 basic states
    1. **Patrolling**: Following a predetermined route
        * **Transition** to **Chasing** state after player enters our line of sight
    2. **Chasing**: Following the player
        * **Transition** to **Searching** state after reaching players last seen location after losing them from the line of sight
    3. **Searching**: Move in random directions looking for player
        * **Transition** to **Patroling** state if after n seconds we have not spotted the player
        * **Transition** to **Chasing** state if we spot the player
        * TODO: instead of moving randomly, have it first move forwards for a few seconds in the last known direction of the player
