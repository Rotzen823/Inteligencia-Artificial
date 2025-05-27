from sklearn.datasets import load_iris
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from tensorflow import keras
import tkinter as tk
from tkinter import messagebox
import numpy as np

#Cargar el dataset Iris
iris = load_iris()
X = iris.data          # 4 características
y = iris.target        # 0, 1 o 2 (clase)

#Normalizar las características
scaler = StandardScaler()
X = scaler.fit_transform(X)

#Dividir en entrenamiento y prueba
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

#Definir la red neuronal
model = keras.Sequential([
    keras.layers.Dense(10, activation='relu', input_shape=(4,)),  # capa oculta
    keras.layers.Dense(10, activation='relu'),                    # otra capa oculta
    keras.layers.Dense(3, activation='softmax')                   # salida (3 clases)
])

#Compilar el modelo
model.compile(
    optimizer='adam',
    loss='sparse_categorical_crossentropy',
    metrics=['accuracy']
)

#Entrenar el modelo
model.fit(
    X_train, y_train,
    epochs=50,
    batch_size=5,
    validation_split=0.2
)

# Evaluar en datos de prueba
loss, acc = model.evaluate(X_test, y_test)
print(f"\nPrecisión en test: {acc:.4f}")

def predecir():
    try:
        idx = int(entry.get())
        if idx < 0 or idx >= len(X_test):
            messagebox.showerror("Error", f"Introduce un índice entre 0 y {len(X_test)-1}")
            return
        
        entrada = X_test[idx].reshape(1, -1)
        prediccion = model.predict(entrada)
        clase = np.argmax(prediccion)
        
        resultado = (f"Índice: {idx}\n"
                     f"Predicción: {iris.target_names[clase]}\n"
                     f"Clase real: {iris.target_names[y_test[idx]]}")
        label_result.config(text=resultado)
    except ValueError:
        messagebox.showerror("Error", "Por favor introduce un número entero válido")

# Crear ventana
root = tk.Tk()
root.title("Predicción de flor Iris")

tk.Label(root, text="Introduce índice de la muestra (0 a {}):".format(len(X_test)-1)).pack(pady=10)

entry = tk.Entry(root)
entry.pack()

btn = tk.Button(root, text="Predecir", command=predecir)
btn.pack(pady=10)

label_result = tk.Label(root, text="", font=("Arial", 12))
label_result.pack(pady=10)

root.mainloop()