import os
import matplotlib.pyplot as plt
import numpy as np

current_dir = os.path.dirname(__file__)
output_dir = os.path.join(current_dir, "..", "..", "output")
filename = os.path.join(output_dir, 'data.txt')

x_coords = []
y_coords = []

try:
    with open(filename, 'r') as f:
        for line in f:
            if line.strip():
                parts = line.split()
                x_coords.append(float(parts[0]))
                y_coords.append(float(parts[1]))
except FileNotFoundError:
    print(f"File '{filename}' doesn't find.")
    exit()

x = np.array(x_coords)
y = np.array(y_coords)

plt.figure(figsize=(8, 6))

plt.plot(x, y, marker='s', color='black', linestyle='-', linewidth=0.7, markerfacecolor='black', markersize=5)

plt.ylabel(r'$\Delta g$')
plt.xlabel('x, м')

plt.grid(True, linestyle=':', color='gray')

plt.xlim(min(x) - 500, max(x) + 500)
plt.ylim((0.95) * min(y), (1.05) * max(y))

plt.show()
