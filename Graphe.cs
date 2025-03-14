﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PSI
{
    public class Graphe
    {

        public Dictionary<Noeud, List<Noeud>> liste_adjacence;
        public int[,] matrice_adjacence;
        public List<Noeud> noeuds { get; private set; }

        public Graphe(int nb_noeuds)
        {
            this.noeuds = new List<Noeud>();
            this.liste_adjacence = new Dictionary<Noeud, List<Noeud>>();
            for(int i = 1; i < nb_noeuds+1; i++)
            {
                Noeud noeud = new Noeud(i);
                noeuds.Add(noeud);
                liste_adjacence[noeud]=new List<Noeud>();
            }
            this.matrice_adjacence = new int[nb_noeuds,nb_noeuds];

        }

        /// <summary>
        /// ajoute un lien de type Lien dans la liste d'adjacence et dans la matrice d'adjacence
        /// </summary>
        /// <param name="lien"></param>
        public void AjouterLien(Lien lien)
        {
            this.liste_adjacence[noeuds[lien.N_depart.Id-1]].Add(noeuds[lien.N_arrivee.Id-1]);
            this.liste_adjacence[noeuds[lien.N_arrivee.Id-1]].Add(noeuds[lien.N_depart.Id-1]);
            this.matrice_adjacence[lien.N_depart.Id-1, lien.N_arrivee.Id-1] = 1;
            this.matrice_adjacence[lien.N_arrivee.Id-1, lien.N_depart.Id-1] = 1;

        }

        public void Afficher_liste_adj()
        {
            foreach(Noeud noeud in this.liste_adjacence.Keys)
            {
                Console.WriteLine(noeud.toString() + "\nLien avec  : ");
                foreach(Noeud lie in liste_adjacence[noeud])
                {
                    Console.Write(lie.toString() + ", ");
                }
                Console.WriteLine("\n");
            }
        }

        public void Affichier_matrice_adj()
        {
            int taille = this.matrice_adjacence.GetLength(0);
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    Console.Write(this.matrice_adjacence[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// parcours en profondeurs du graphe utilisant la liste d'adjacence
        /// </summary>
        /// <param name="depart"></param>
        public bool[] DFS(Noeud depart)
        {
            Stack<Noeud> pile = new Stack<Noeud>();
            bool[] visite = new bool[noeuds.Count];
            List<Noeud> ordreVisites = new List<Noeud>();

            pile.Push(depart);

            while(pile.Count > 0)
            {
                Noeud actuel = pile.Pop();
                int index = actuel.Id - 1;
                if(visite[index] == false)
                {
                    visite[index]  = true;
                    ordreVisites.Add(actuel);
                    foreach(Noeud voisin in this.liste_adjacence[actuel])
                    {
                        int voisinIndex = voisin.Id - 1;
                        if (visite[voisinIndex]== false)
                        {
                            pile.Push(voisin);
                        }
                    }
                }
            }

            // Affichage de l'ordre des nœuds visités
            Console.WriteLine("Ordre des noeuds visités : ");
            foreach (Noeud noeud in ordreVisites)
            {
                Console.Write(noeud.toString() + " ");
            }
            Console.WriteLine();

            return visite;

        }

        /// <summary>
        /// parcours en largeurs du graphe en utilisant la liste d'adjacence
        /// </summary>
        /// <param name="depart"></param>
        public bool[] BFS(Noeud depart)
        {
            bool[] visite = new bool[noeuds.Count];
            Queue<Noeud> file = new Queue<Noeud>();
            List<Noeud> ordreVisites = new List<Noeud>();

            file.Enqueue(depart);
            visite[depart.Id-1] = true;

            while(file.Count > 0)
            {
                Noeud actuel = file.Dequeue();
                ordreVisites.Add(actuel) ;

                foreach(Noeud voisin in liste_adjacence[actuel] )
                {
                    if (visite[voisin.Id -1]== false)
                    {
                        visite[voisin.Id -1] = true;
                        file.Enqueue(voisin);
                    }
                }
            }

            Console.WriteLine("Ordre des noeuds visités : ");
            foreach (Noeud noeud in ordreVisites)
            {
                Console.Write(noeud.toString() + " ");
            }
            Console.WriteLine();
            return visite;
        }

        public bool EstConnexe()
        {
            bool est_connexe = true;
            for(int i = 0;i<this.noeuds.Count;i++)
            {
                bool[] visite = this.DFS(this.noeuds[i]);
                foreach(bool val in  visite)
                {
                    if (val == false)
                    {
                        est_connexe = false;
                    }
                }
            }
            return est_connexe;

        }
    }
}
