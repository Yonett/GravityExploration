import os
import matplotlib.pyplot as plt
import numpy as np

def read_data(filename):
    x_coords = []
    y_coords = []
    try:
        with open(filename, 'r') as f:
            for line in f:
                if line.strip():
                    try:
                        parts = line.split()
                        x_coords.append(float(parts[0]))
                        y_coords.append(float(parts[1]))
                    except (ValueError, IndexError):
                        print(f"{filename}: '{line.strip()}'")
    except FileNotFoundError:
        print(f"Error: file '{filename}' doesn't find.")
        return None, None
    return x_coords, y_coords

current_dir = os.path.dirname(__file__)
output_dir = os.path.join(current_dir, "..", "..", "output")

file1 = os.path.join(output_dir, 'data.txt')
file2 = os.path.join(output_dir, 'reg1.txt')
file3 = os.path.join(output_dir, 'reg2.txt')

x1, y1 = read_data(file1)
x2, y2 = read_data(file2)
x3, y3 = read_data(file3)

if x1 is None or x2 is None or x3 is None:
    exit()

plt.figure(figsize=(10, 7))
plt.grid(True, linestyle=':', color='gray')
plt.plot(x1, y1,
         marker='s',              
         linestyle='-',           
         linewidth=0.5,           
         color='gray',            
         markerfacecolor='gray',  
         markeredgecolor='black', 
         markersize=5,            
         label="WR")

plt.plot(x2, y2,
         marker='v',              
         linestyle='-',
         linewidth=0.5,
         color='black',
         markerfacecolor='black',
         markeredgecolor='black',
         markersize=6,
         label=r'REG $\alpha = 10^{-3}$')

plt.plot(x3, y3,
         marker='v',
         linestyle='-',
         linewidth=0.5,
         color='darkgray',
         markerfacecolor='none',  
         markeredgecolor='darkgray',
         markersize=6,
         label=r'REG $\alpha = 10^{-1}$')

plt.xlabel('x,m')
plt.ylabel('%')

plt.xlim(0, 6000)
plt.ylim(-1.8, 1.8)
plt.xticks([0, 2000, 4000, 6000])
plt.yticks([-100.0, 0.0, 100.0])
plt.legend()
caption = ()
plt.figtext(0.5, -0.05, caption, wrap=True, horizontalalignment='center', fontsize=12)

plt.tight_layout(rect=[0, 0.1, 1, 1])

plt.show()
