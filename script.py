
import os
import matplotlib.pyplot as plt
import matplotlib.patches as patches

# Configuración de la carpeta de salida (guardar dentro de la carpeta `wwwroot/imagenes/cartas`)
script_dir = os.path.dirname(os.path.abspath(__file__))
output_dir = os.path.join(script_dir, 'wwwroot', 'imagenes', 'cartas')
os.makedirs(output_dir, exist_ok=True)

# Palos con la paleta de alto contraste de Balatro
suits = {
    'Corazones': {'symbol': '♥', 'color': '#ff2a4b'},  # Rojo
    'Diamantes': {'symbol': '♦', 'color': '#ffc107'},  # Amarillo
    'Treboles':  {'symbol': '♣', 'color': '#0055ff'},  # Azul
    'Picas':     {'symbol': '♠', 'color': '#1c2024'}   # Negro/Oscuro
}
ranks = ['A', '2', '3', '4', '5', '6', '7', '8', '9', '10', 'J', 'Q', 'K']

def draw_card(rank, suit_name, suit_info, filename):
    fig, ax = plt.subplots(figsize=(3.5, 5))
    ax.set_xlim(0, 3.5)
    ax.set_ylim(0, 5)
    ax.axis('off')
    
    # Dibujar el contorno y fondo blanco con esquinas redondeadas
    card_bg = patches.FancyBboxPatch(
        (0.1, 0.1), 3.3, 4.8, 
        boxstyle="round,pad=0,rounding_size=0.2", 
        edgecolor='#2c2c2c', facecolor='white', linewidth=2
    )
    ax.add_patch(card_bg)
    
    color = suit_info['color']
    symbol = suit_info['symbol']
    
    # Marca de agua de fondo para las figuras (J, Q, K)
    if rank in ['J', 'Q', 'K']:
        ax.text(1.75, 2.5, rank, fontsize=90, color=color, ha='center', va='center', weight='bold', alpha=0.08)
    
    # Símbolo grande central: mostrar palo + número/letra juntos
    # Ejemplo: 'A♥', '10♣', 'Q♦'
    central_text = f"{rank}{symbol}"
    ax.text(1.75, 2.5, central_text, fontsize=90, color=color, ha='center', va='center')
    
    # Índice superior izquierdo
    ax.text(0.35, 4.5, rank, fontsize=24, color=color, ha='center', va='center', weight='bold')
    ax.text(0.35, 4.1, symbol, fontsize=20, color=color, ha='center', va='center')
    
    # Índice inferior derecho (rotado 180 grados)
    ax.text(3.15, 0.5, rank, fontsize=24, color=color, ha='center', va='center', weight='bold', rotation=180)
    ax.text(3.15, 0.9, symbol, fontsize=20, color=color, ha='center', va='center', rotation=180)
    
    # Guardar con dimensiones estrictas y fondo transparente
    plt.savefig(os.path.join(output_dir, filename), dpi=150, transparent=True, pad_inches=0)
    plt.close()

# Generación de las 52 cartas
print("Generando baraja Balatro (Alto Contraste)...")
for suit_name, suit_info in suits.items():
    for rank in ranks:
        filename = f"{rank}_{suit_name}.png"
        draw_card(rank, suit_name, suit_info, filename)

print(f"¡Listo! Se han guardado las 52 imágenes en la carpeta '{output_dir}'.")

