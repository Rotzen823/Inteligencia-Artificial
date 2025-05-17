import tkinter as tk
from tkinter import messagebox
import numpy as np
import skfuzzy as fuzz
from skfuzzy import control as ctrl
import matplotlib.pyplot as plt

# Variables difusas
goles = ctrl.Antecedent(np.arange(0, 6, 1), 'Goles')
asistencias = ctrl.Antecedent(np.arange(0, 6, 1), 'Asistencias')
pases = ctrl.Antecedent(np.arange(0, 51, 1), 'PasesCorrectos')
recuperaciones = ctrl.Antecedent(np.arange(0, 31, 1), 'Recuperaciones')
faltas = ctrl.Antecedent(np.arange(0, 11, 1), 'Faltas')
rendimiento = ctrl.Consequent(np.arange(0, 101, 1), 'Rendimiento')

pases['pocos'] = fuzz.trimf(pases.universe, [0, 0, 20])
pases['aceptables'] = fuzz.trimf(pases.universe, [10, 25, 40])
pases['muchos'] = fuzz.trimf(pases.universe, [30, 50, 50])

goles['ninguno'] = fuzz.trimf(goles.universe, [0, 0, 1])
goles['pocos'] = fuzz.trimf(goles.universe, [0, 2, 4])
goles['varios'] = fuzz.trimf(goles.universe, [3, 5, 5])

asistencias['ninguno'] = fuzz.trimf(asistencias.universe, [0, 0, 3])
asistencias['pocos'] = fuzz.trimf(asistencias.universe, [0, 2, 4])
asistencias['varios'] = fuzz.trimf(asistencias.universe, [3, 5, 5])

faltas['pocas'] = fuzz.trimf(faltas.universe, [0, 0, 3])
faltas['algunas'] = fuzz.trimf(faltas.universe, [2, 5, 8])
faltas['muchas'] = fuzz.trimf(faltas.universe, [6, 10, 10])

recuperaciones['bajas'] = fuzz.trimf(recuperaciones.universe, [0, 0, 10])
recuperaciones['moderadas'] = fuzz.trimf(recuperaciones.universe, [5, 15, 25])
recuperaciones['altas'] = fuzz.trimf(recuperaciones.universe, [20, 30, 30])

rendimiento['bajo'] = fuzz.trimf(rendimiento.universe, [0, 0, 40])
rendimiento['medio'] = fuzz.trimf(rendimiento.universe, [30, 50, 70])
rendimiento['alto'] = fuzz.trimf(rendimiento.universe, [60, 100, 100])

# Reglas
reglas = [
    # Alto rendimiento ofensivo
    ctrl.Rule(goles['varios'] & asistencias['varios'], rendimiento['alto']),
    ctrl.Rule(goles['pocos'] & asistencias['pocos'] & pases['muchos'], rendimiento['alto']),
    ctrl.Rule(goles['pocos'] & pases['muchos'] & faltas['pocas'], rendimiento['alto']),

    # Medio rendimiento ofensivo
    ctrl.Rule(goles['pocos'] & asistencias['pocos'] & faltas['algunas'], rendimiento['medio']),
    ctrl.Rule(asistencias['pocos'] & pases['aceptables'], rendimiento['medio']),
    
    # Buen defensor
    ctrl.Rule(recuperaciones['altas'] & faltas['pocas'], rendimiento['alto']),
    ctrl.Rule(recuperaciones['moderadas'] & pases['aceptables'], rendimiento['medio']),
    
    # Penalización por faltas
    ctrl.Rule(faltas['muchas'] & goles['ninguno'], rendimiento['bajo']),
    ctrl.Rule(faltas['muchas'] & asistencias['ninguno'], rendimiento['bajo']),
    ctrl.Rule(faltas['muchas'] & pases['pocos'], rendimiento['bajo']),

    # Jugador sin impacto
    ctrl.Rule((goles['ninguno'] & asistencias['ninguno']) & pases['pocos'], rendimiento['bajo']),
    ctrl.Rule(recuperaciones['bajas'] & faltas['algunas'] & asistencias['ninguno'], rendimiento['bajo']),
]

sistema = ctrl.ControlSystem(reglas)
simulador = ctrl.ControlSystemSimulation(sistema)

def evaluar():
    try:
        simulador.input['PasesCorrectos'] = int(entry_pases.get())
        simulador.input['Goles'] = int(entry_goles.get())
        simulador.input['Faltas'] = int(entry_faltas.get())
        simulador.input['Recuperaciones'] = int(entry_recuperaciones.get())
        simulador.input['Asistencias'] = int(entry_asistencias.get())

        simulador.compute()
        resultado = simulador.output['Rendimiento']
        messagebox.showinfo("Resultado", f"Rendimiento del jugador: {resultado:.2f}/100")
        rendimiento.view(sim=simulador)
        plt.show()
    except Exception as e:
        messagebox.showerror("Error", str(e))

root = tk.Tk()
root.title("Evaluador de Rendimiento de Jugador")

tk.Label(root, text="Goles (0-5):").grid(row=0, column=0)
entry_goles = tk.Entry(root)
entry_goles.grid(row=0, column=1)
entry_goles.insert(0, "1")

tk.Label(root, text="Asistencias (0-5):").grid(row=1, column=0)
entry_asistencias = tk.Entry(root)
entry_asistencias.grid(row=1, column=1)
entry_asistencias.insert(0, "1")

tk.Label(root, text="Pases Correctos (0-50):").grid(row=2, column=0)
entry_pases = tk.Entry(root)
entry_pases.grid(row=2, column=1)
entry_pases.insert(0, "50")

tk.Label(root, text="Recuperaciones (0-30):").grid(row=3, column=0)
entry_recuperaciones = tk.Entry(root)
entry_recuperaciones.grid(row=3, column=1)
entry_recuperaciones.insert(0, "20")

tk.Label(root, text="Faltas (0-10):").grid(row=4, column=0)
entry_faltas = tk.Entry(root)
entry_faltas.grid(row=4, column=1)
entry_faltas.insert(0, "0")

tk.Button(root, text="Evaluar", command=evaluar).grid(row=5, columnspan=2, pady=10)

root.mainloop()