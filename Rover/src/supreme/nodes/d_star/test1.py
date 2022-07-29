from d_star import DStar
import numpy as np


a = np.loadtxt("input.txt", dtype='i', delimiter=',')
# b = [[0, 1, 1, 1, 1], [0, 1, 1, 1, 1], [0, 1, 1, 1, 1], [0, 1, 1, 1, 1], [0, 0, 0, 0, 0]]
pf = DStar(x_start=0, y_start=0, x_goal=len(a), y_goal=len(a[i]))

for i in range(len(a)):
    for j in range(len(a[i])):
       if a[i][j] == 1:
            pf.update_cell(i+1, j+1, -1)
       else:
            pf.update_cell(i+1, j+1, 0)

pf.replan()
path = pf.get_path()

# for i in range len(path):
#    print("x: " + i.x + " y: " + i.y)

## Java uygulamasi (below)
//Create pathfinder
  DStarLite pf = new DStarLite();
  //set start and goal nodes
  pf.init(0,1,3,1);
  //set impassable nodes
  pf.updateCell(2, 1, -1);
  pf.updateCell(2, 0, -1);
  pf.updateCell(2, 2, -1);
  pf.updateCell(3, 0, -1);

  //perform the pathfinding
  pf.replan();

  //get and print the path
  List<State> path = pf.getPath();
  for (State i : path)
  {   
     System.out.println("x: " + i.x + " y: " + i.y);
  }   

