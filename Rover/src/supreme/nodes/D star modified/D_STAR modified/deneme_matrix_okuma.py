```python
from d_star import DStar

/*
# setting start and end points /// start position: from gps , goal: from user
pf = DStar(x_start=0, y_start=1, x_goal=3, y_goal=1)

# making cell unpassable ////engel görünce
pf.update_cell(2, 1, -1)

# making cell passable
pf.update_cell(2, 1, 0)

# recalculating path
pf.replan()
path = pf.get_path()
```

A couple of useful functions:

```python
pf.update_start(x, y)
pf.update_goal(x, y)
```
*/

-----------------------------------------------
Ornek:
>>> x = np.random.randint(9, size=(3, 3))
>>> x
array([[3, 1, 7],
       [2, 8, 3],
       [8, 5, 3]])
>>> x.item(3)
2
>>> x.item(7)
5
>>> x.item((0, 1))
1
>>> x.item((2, 2))             //item returns Standard Python scalar object (convert it into integer!)
-------------------------------------------------
Ornek2:

a = [[1, 2, 3, 4], [5, 6], [7, 8, 9]]
for i in range(len(a)):
    for j in range(len(a[i])):
        print(a[i][j], end=' ')
    print()
-------------------------------------------------
Ornek3:

from d_star import DStar

a: sensorden gelen matrix
b: initially empty matrix

//aşağıdaki for looptan önce a matrixini çekmemiz gerekiyor!!!!!

for i in range(len(a)):
    for j in range(len(a[i])):
       b[i][j] == (a[i][j])*(-1)   // eğer a matrixindeki bir element 0 ise bde de 0 olacak. 1 ise -1 olacak(yani engeli var)!! buna gerek kalmayabilir
       if (a[i][j] == 1)
              pf.update_cell(i, j, -1)
       else
              pf.update_cell(i, j, 0)

pf.replan()
path = pf.get_path()


-------------------------------------------------

sürekli file'dan matrix çekiyoruz.
if (initial matrix is not equal to next matrix)
       update path 