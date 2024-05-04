import numpy as np
import matplotlib.pyplot as plt

# Дефиниране на функцията за тричлен
def quadratic(x):
    return x**2 + 4*x + 7

# Генериране на точки за x в интервала [-2, 1]
x_values = np.linspace(-2, 1, 100)
y_values = quadratic(x_values)

# Изчертаване на графиката
plt.plot(x_values, y_values, label='x^2 + 4x + 7')
plt.xlabel('x')
plt.ylabel('y')
plt.title('Graph of x^2 + 4x + 7')
plt.grid(True)
plt.legend()
plt.axvline(x=-2, color='r', linestyle='--', label='x = -2')
plt.axvline(x=1, color='g', linestyle='--', label='x = 1')
plt.legend()
plt.show()
