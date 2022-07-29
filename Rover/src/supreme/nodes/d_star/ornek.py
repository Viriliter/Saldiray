from d_star import DStar

# setting start and end points /// start position: from gps , goal: from user
pf = DStar(x_start=0, y_start=1, x_goal=3, y_goal=1)

# making cell unpassable ////engel görünce
pf.update_cell(2, 1, -1)

# making cell passable
pf.update_cell(2, 1, 0)

# recalculating path
pf.replan()
path = pf.get_path()
print path
