# 🚀 Space Invaders ADS

Um projeto simples e divertido inspirado no clássico **Space Invaders**, desenvolvido em **C# Console Application** para a disciplina de **Análise e Desenvolvimento de Sistemas (ADS)**.

---

## 🎮 Sobre o Projeto

O jogo foi desenvolvido utilizando conceitos fundamentais de:

- Programação Orientada a Objetos (POO)
- Estruturas de repetição
- Manipulação de listas
- Controle de colisão
- Game Loop
- Manipulação de teclado no Console
- Refatoração e organização de código

O objetivo do jogador é destruir todos os aliens antes que eles alcancem a nave.

---

# 🕹️ Gameplay

- Controle a nave utilizando:
  - ⬅️ `Seta Esquerda`
  - ➡️ `Seta Direita`
- Atire utilizando:
  - 🔫 `Barra de Espaço`
- Fechar o jogo:
  - ❌ `ESC`

---

# 📸 Funcionalidades

✅ Sistema de movimentação da nave  
✅ Sistema de tiros com cooldown  
✅ Movimento coletivo dos aliens  
✅ Sistema de colisão  
✅ Sistema de vidas  
✅ Sistema de pontuação  
✅ Tela de vitória  
✅ Tela de derrota  
✅ HUD dinâmica  
✅ Sons utilizando `Console.Beep()`  
✅ Código refatorado e otimizado  

---

# 🏗️ Estrutura do Projeto

```bash
SpaceInvadersADS/
│
├── SpaceInvadersADS.cs
├── README.md
```

---

# 🧠 Estrutura de Classes

## 🔹 Entidade (Classe Abstrata)

Classe base responsável pelas propriedades comuns:

- Posição X/Y
- Símbolo
- Cor

---

## 🔹 Nave

Responsável por:

- Movimentação do jogador
- Controle de vidas
- Limite de tela

---

## 🔹 Alien

Representa os inimigos do jogo.

---

## 🔹 Tiro

Responsável pela lógica dos disparos.

---

# ⚙️ Tecnologias Utilizadas

- Linguagem: **C#**
- Plataforma: **.NET**
- Interface: **Console Application**

---

# ▶️ Como Executar

## 1️⃣ Clone o repositório

```bash
git clone https://github.com/seu-usuario/SpaceInvadersADS.git
```

---

## 2️⃣ Entre na pasta do projeto

```bash
cd SpaceInvadersADS
```

---

## 3️⃣ Compile e execute

```bash
dotnet run
```

---

# 📚 Conceitos Aplicados

- Programação Orientada a Objetos
- Encapsulamento
- Herança
- Polimorfismo
- LINQ
- Estruturas condicionais
- Estruturas de repetição
- Manipulação de coleções
- Controle de FPS

---

# 📈 Histórico de Versões

| Data       | Alteração |
|------------|------------|
| 12/03/2026 | Criação do código inicial |
| 14/03/2026 | Refatoração das Classes de Entidades |
| 15/03/2026 | Ajuste do tamanho da tela e FPS |
| 18/03/2026 | Criação da Tela de Vitória e Derrota |
| 19/03/2026 | Refatoração completa e otimização |

---

# 👨‍💻 Autor

**Luis Felipe Dias**

Projeto acadêmico desenvolvido para estudos e prática de lógica de programação e C#.

---

# 📄 Licença

Este projeto é livre para estudos e modificações.
