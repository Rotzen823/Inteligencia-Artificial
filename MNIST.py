import tensorflow as tf
from tensorflow import keras
import numpy as np
import matplotlib.pyplot as plt

# Cargar el dataset MNIST
(x_train, y_train), (x_test, y_test) = keras.datasets.mnist.load_data()

# Preprocesamiento
# Normalizar los valores de píxeles a [0, 1]
x_train = x_train.astype("float32") / 255.0
x_test = x_test.astype("float32") / 255.0

# Definir el modelo
model = keras.Sequential([
    keras.layers.Flatten(input_shape=(28, 28)),  # Aplanar la imagen 28x28
    keras.layers.Dense(128, activation='relu'),
    keras.layers.Dropout(0.2),
    keras.layers.Dense(10, activation='softmax')  # 10 clases (dígitos 0-9)
])

# Compilar el modelo
model.compile(
    optimizer='adam',
    loss='sparse_categorical_crossentropy',
    metrics=['accuracy']
)

# Entrenar el modelo
history = model.fit(
    x_train, y_train,
    epochs=10,
    batch_size=32,
    validation_split=0.2
)

# Evaluar el modelo
test_loss, test_acc = model.evaluate(x_test, y_test, verbose=2)
print(f"\nPrecisión en test: {test_acc:.4f}")

# Mostrar una predicción de ejemplo
def plot_prediction(index):
    plt.imshow(x_test[index], cmap='gray')
    pred = np.argmax(model.predict(x_test[index][np.newaxis, ...]))
    plt.title(f"Predicción: {pred}, Real: {y_test[index]}")
    plt.axis('off')
    plt.show()

plot_prediction(6)  # Cambiar el índice para probar imágenes