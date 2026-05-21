/*********************************************************************
 * Arquivo.....: SpaceInvadersADS.cs
 * Descrição...: Implementação básica do jogo Space Invaders em C#
 * Autor.......: Luis Felipe Dias
 * Data........: 12/03/2026
 * Versão......: 1.0
 * ===================================================================
 * 12/03/2026       | Criação do código inicial
 * 14/03/2026       | Refatoração das Classes de Entidades
 * 15/03/2026       | Ajuste do tamanho da tela e FPS
 * 18/03/2026       | Criação da Tela de Vitória e Derrota
 * 19/03/2026       | Refatoração do código completo, otimizando loops
 *                  | e funções
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace SpaceInvadersADS
{
    /* =============================================================================
       Criação das classes principais, Abstrata que contém todas as funções básicas
       ============================================================================= */

    abstract class Entidade
    {
        /* X é a direção horizontal (coluna) */
        public int X { get; set; }
        /* Y é a direção vertical (linha) */
        public int Y { get; set; }
        /* Simbolo é o caractere ou emoji que representa a entidade na tela */
        public string Simbolo { get; protected set; }
        /* Cor é a cor do símbolo quando desenhado no console */
        public ConsoleColor Cor { get; protected set; }
    }

    class Nave : Entidade
    {
        public int Vidas { get; private set; }

        public Nave(int larguraTela)
        {
            X = larguraTela / 2;
            Y = 18;                    /* Fica perto da parte inferior */
            Simbolo = "A";             /* 'A' para garantir compatibilidade, pode ser emoji */
            Cor = ConsoleColor.Cyan;
            Vidas = 3;
        }

        /* O Math.Max impede que o X fique negativo (bateu na parede esquerda) */
        public void MoverEsquerda()
        {
            X = Math.Max(0, X - 1);
        }

        /* O Math.Min impede que o X passe da largura máxima da tela */
        public void MoverDireita(int largura)
        {
            X = Math.Min(largura - 1, X + 1);
        }

        public void PerderVida()
        {
            Vidas--;
        }
        public bool EstaVivo => Vidas > 0;
    }

    class Alien : Entidade
    {
        public Alien(int x, int y)
        {
            X = x;
            Y = y;
            Simbolo = "W";             /* Símbolo do Alien, pode ser o emoji */
            Cor = ConsoleColor.Magenta;
        }
    }

    class Tiro : Entidade
    {
        public Tiro(int x, int y)
        {
            X = x;
            Y = y;
            Simbolo = "*";
            Cor = ConsoleColor.Yellow;
        }

        /* O tiro sobe na tela, logo o Y diminui */
        public void Avancar()
        {
            Y--;
        }
    }

    /* ======================================================================
       O programa principal,estrutura e lógica de jogo, desde a configuração 
       da Tela, ao Loop que garante que o jogo funcione sem falhas.
       ====================================================================== */

    class Program
    {
        const int LARGURA = 50;
        const int ALTURA = 20;
        const int FPS_DELAY_MS = 17;     /* ~60 FPS */

        static void Main()
        {
            /* Configuração inicial do console */
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            /* Para de alertar sobre possíveis problemas */
#pragma warning disable CA1416
            Console.SetWindowSize(LARGURA, ALTURA + 3);
            Console.SetBufferSize(LARGURA, ALTURA + 3);
#pragma warning restore CA1416

            /* 1. Criação dos objetos */
            var nave = new Nave(LARGURA);
            var tiros = new List<Tiro>();
            var aliens = new List<Alien>();

            /* Cria 2 fileiras de aliens usando loop aninhado */
            for (int linha = 1; linha <= 2; linha++)
            {
                for (int col = 4; col < LARGURA - 4; col += 5)
                {
                    aliens.Add(new Alien(col, linha));
                }
            }

            bool jogando = true;
            int pontos = 0;

            /* Variáveis de controle de fluxo e cadência */
            int direcaoAliens = 1;        /* 1 = direita, -1 = esquerda */
            int contadorMovimentoAlien = 0;
            int maxTirosSimultaneos = 3;
            DateTime ultimoTiro = DateTime.MinValue;

            /* O GAME LOOP */
            while (jogando)
            {
                /* INPUT (LER O TECLADO) */
                if (Console.KeyAvailable)
                {
                    var tecla = Console.ReadKey(true).Key;

                    if (tecla == ConsoleKey.LeftArrow) nave.MoverEsquerda();
                    if (tecla == ConsoleKey.RightArrow) nave.MoverDireita(LARGURA);

                    /* Controle de Fogo: Limita os tiros simultâneos ("cooldown" de 180ms) */
                    if (tecla == ConsoleKey.Spacebar &&
                        tiros.Count < maxTirosSimultaneos &&
                        (DateTime.Now - ultimoTiro).TotalMilliseconds > 180)
                    {
                        tiros.Add(new Tiro(nave.X, nave.Y - 1));
                        ultimoTiro = DateTime.Now;
#pragma warning disable CA1416
                        Console.Beep(1200, 60);     /* Efeito sonoro do tiro */
#pragma warning restore CA1416
                    }

                    if (tecla == ConsoleKey.Escape) jogando = false;
                }

                /* ATUALIZAR LÓGICA */

                /* Movimenta os Tiros */
                foreach (var t in tiros) t.Avancar();

                /* LINQ: Remove tiros que saíram pelo topo da tela */
                tiros.RemoveAll(t => t.Y < 0);

                /* Movimento dos Aliens (Mais lentos que o jogo normal) */
                contadorMovimentoAlien++;
                if (contadorMovimentoAlien >= 12)
                {
                    contadorMovimentoAlien = 0;

                    /* Verifica se a frota toda precisa descer e virar */
                    bool deveDescer = false;
                    foreach (var a in aliens)
                    {
                        int novoX = a.X + direcaoAliens;
                        /* Se algum alien encostou nas bordas laterais */
                        if (novoX <= 1 || novoX >= LARGURA - 2)
                        {
                            deveDescer = true;
                            break;
                        }
                    }

                    /* 2. Executa o movimento em bloco */
                    if (deveDescer)
                    {
                        direcaoAliens *= -1;        /* Inverte a direção matematicamente */
                        foreach (var a in aliens) a.Y++; /* Desce uma linha */
                    }
                    else
                    {
                        foreach (var a in aliens) a.X += direcaoAliens; /* Move lateralmente */
                    }
                }

                /* COLISÕES */

                /* Tiro atinge Alien */
                /* .ToList() para criar uma cópia da lista de tiros. Se não fizer isso, 
                 * o C# dá erro ao tentar remover um item da lista enquanto percorre */
                foreach (var tiro in tiros.ToList())
                {
                    /* Busca o primeiro alien que está no mesmo X e Y do tiro atual */
                    var atingido = aliens.Find(a => a.X == tiro.X && a.Y == tiro.Y);

                    if (atingido != null)
                    {
                        aliens.Remove(atingido);
                        tiros.Remove(tiro);
                        pontos += 100;
#pragma warning disable CA1416
                        Console.Beep(800, 80);   /* Som de explosão */
#pragma warning restore CA1416
                    }
                }

                /* Alien atinge o limite inferior da tela (Chegou perto da Nave) */
                if (aliens.Any(a => a.Y >= nave.Y - 1))
                {
                    nave.PerderVida();

                    if (!nave.EstaVivo)
                    {
                        jogando = false;
                        MostrarTelaFinal("GAME OVER", pontos, nave.Vidas);
                        continue;
                    }

                    /* Se ainda tem vidas, "empurra" os aliens para cima para dar uma chance */
                    foreach (var a in aliens) a.Y = Math.Max(1, a.Y - 3);
                }

                /* Vitória! (Catálogo de aliens zerou) */
                if (aliens.Count == 0)
                {
                    MostrarTelaFinal("VOCÊ VENCEU!", pontos, nave.Vidas);
                    jogando = false;
                    continue;
                }

                /* DESENHAR NA TELA */
                Console.Clear();

                /* Desenha o Jogador */
                Desenhar(nave.X, nave.Y, nave.Simbolo, nave.Cor);

                /* Desenha os Aliens */
                foreach (var a in aliens) Desenhar(a.X, a.Y, a.Simbolo, a.Cor);

                /* Desenha os Tiros */
                foreach (var t in tiros) Desenhar(t.X, t.Y, t.Simbolo, t.Cor);

                /* HUD (Heads-Up Display) - Placar e Vidas */
                Console.SetCursorPosition(1, ALTURA);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" Pontos: {pontos,-6}  Vidas: ");

                Console.ForegroundColor = ConsoleColor.Red;
                for (int i = 0; i < nave.Vidas; i++)
                {
                    Console.Write("♥ ");
                }
                Console.ResetColor();

                /* Controla a velocidade do jogo (FPS) */
                Thread.Sleep(FPS_DELAY_MS);
            }

            Console.CursorVisible = true;
            Console.ReadLine();
        }

        /* Método Auxiliar de Desenho Seguro */
        static void Desenhar(int x, int y, string simbolo, ConsoleColor cor)
        {
            if (x < 0 || x >= LARGURA || y < 0 || y >= ALTURA) return;

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = cor;
            Console.Write(simbolo);
            Console.ResetColor();
        }

        /* Tela Fim de Jogo */
        static void MostrarTelaFinal(string titulo, int pontos, int vidas)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(8, 6);
            Console.WriteLine(titulo);
            Console.SetCursorPosition(8, 8);
            Console.WriteLine($"Pontuação final: {pontos}");
            Console.SetCursorPosition(8, 9);
            Console.WriteLine($"Vidas restantes: {vidas}");
            Console.ResetColor();
            Console.SetCursorPosition(8, 12);
            Console.WriteLine("Pressione ENTER para sair...");
        }
    }
}