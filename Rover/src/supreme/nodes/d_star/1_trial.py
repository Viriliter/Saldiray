from d_star import DStar
import numpy as np


a = [[0, 1, 1, 1], [0, 1, 1, 1], [0, 1, 1, 1], [0, 0, 0, 0]]
# a = np.loadtxt("input.txt", dtype='i', delimiter=',')
pf = DStar(x_start=0, y_start=0, x_goal=4, y_goal=4)

for i in range(len(a)):
    for j in range(len(a[i])):
       if a[i][j] == 1:
            pf.update_cell(i+1, j+1, -1)
       else:
            pf.update_cell(i+1, j+1, 0)

pf.replan()
path = pf.get_path()
for k in range(len(path)):
     print("x: " + i.x + " y: " + i.y)