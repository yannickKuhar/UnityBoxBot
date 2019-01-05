using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ai : MonoBehaviour {

	public Box box;
	public Goal goal;
	public GameObject spawnPoint;
	public Player player;
	public GameObject boxStartPoint;
	private Player[] population;
	private int count = 0;
	private int generation = 1;
	private bool spawnNextGen;
	private bool updateFun;

	// Use this for initialization
	void Start ()
	{
		spawnNextGen = true;
		updateFun = true;

		boxStartPoint = GameObject.Find("BoxStartPoint");

		population = new Player[128];

		for(int i = 0; i < population.Length; i++)
		{
			population[i] = player;
			population[i].InitPlayer();
		}
	}

	IEnumerator Delay()
    {
        print(Time.time);
        yield return new WaitForSecondsRealtime(5);
        print(Time.time);
    }
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if(spawnNextGen)
		{
			if(count < population.Length)
			{	
				Instantiate(population[count], spawnPoint.transform.position, spawnPoint.transform.rotation);
				count++;
			}
			if(count >= population.Length) 
			{	
				// StartCoroutine(Delay());
				// Debug.Log(generation);

				for(int i = 0; i < 128; i++)
				{
					population[i].SetFitness(box, goal);
				}

				population = GeneticAlgorithm(population);

				count = 0;
				generation++;

				IncreaseGenCount();

				box.transform.position = boxStartPoint.transform.position;
				box.transform.rotation = boxStartPoint.transform.rotation;
				box.RemoveForce();	
			}
		}

		if(goal.GetHit() && updateFun)
		{
			spawnNextGen = false;
			updateFun = false;
			DisplayResult(generation);
			// Debug.Log("Resitev najdena v " + generation + ". generaciji!");
		}
	}

	void Mutation(Player agent)
	{
		// Mutacija se zgodi, če je rezultat meta 10 strane kocke 5.
		if((int)UnityEngine.Random.Range(1, 10) == 5)
		{
			int n = agent.GetGenom().Length;

			// Debug.Log("Genom length: " + n);

			// Chose gene to mutate.
			int idx = (int) UnityEngine.Random.Range(0, (n - 1));

			// Debug.Log("Mutation index: " + idx);

			float[] newGenom = agent.GetGenom();
			newGenom[idx] = UnityEngine.Random.Range(-5, 5);
		
			agent.SetGenom(newGenom);
		}
	}

	// Single point.
	Player[] Crossover(Player parent1, Player parent2) 
	{
		int n = parent1.GetGenom().Length;

		// Debug.Log(parent1.GetFitness() + " " + parent2.GetFitness());

		// Izberemo indeks rezanja.
		int slice = (int) UnityEngine.Random.Range(1, (n - 1));

		// Naredimo kopije genomov, da preprecimo 
		// napake tipa read-write.
		float[] p1 = parent1.GetGenom();
		float[] p2 = parent2.GetGenom();
		float[] child1 = parent1.GetGenom();
		float[] child2 = parent2.GetGenom();
		
		for(int i = 0; i < slice; i++)
		{
			child1[i] = p2[i];
		}

		for(int i = slice; i < n; i++)
		{
			child2[i] = p1[i];
		}

		// V starse shranimo genoma otrok
		parent1.SetGenom(child1);
		parent2.SetGenom(child2);

		Player[] result = new Player[2];
		result[0] = parent1;
		result[1] = parent2;

		return result;
	}

	// Ta crossover zanemari prva 2 elementa genoma.
	Player[] CrossoverWithoutHead(Player parent1, Player parent2)
	{
		int n = parent1.GetGenom().Length;

		// Debug.Log(parent1.GetFitness() + " " + parent2.GetFitness());

		int slice = (int) UnityEngine.Random.Range(3, (n - 1));

		float[] p1 = parent1.GetGenom();
		float[] p2 = parent2.GetGenom();
		float[] child1 = parent1.GetGenom();
		float[] child2 = parent2.GetGenom();

		if(parent1.GetFitness() > parent2.GetFitness())
		{
			child2[0] = child1[0];
			child2[1] = child1[1];
		}
		else
		{
			child1[0] = child2[0];
			child1[1] = child2[1];
		}
		
		for(int i = 2; i < slice; i++)
		{
			child1[i] = p2[i];
		}

		for(int i = slice; i < n; i++)
		{
			child2[i] = p1[i];
		}

		parent1.SetGenom(child1);
		parent2.SetGenom(child2);

		Player[] result = new Player[2];
		result[0] = parent1;
		result[1] = parent2;

		return result;
	}

	int TournamentSelection(Player[] pop, int k)
	{
		int best = 0;

		for(int i = 0; i < k; i++)
		{
			int contender = (int) UnityEngine.Random.Range(0, (pop.Length - 1));

			if(best == 0 || pop[contender].GetFitness() <= pop[best].GetFitness())
			{
				best = contender;
			}
		}

		return best;
	} 

	Player[] GeneticAlgorithm(Player[] populateion)
	{
		Player[] nextGen = new Player[populateion.Length];
		Player[] subPop = new Player[4];

		for(int i = 0; i < populateion.Length; i += 4)
		{
			// Z okencem velikosti 4 se pomikamo po
			// celotni popoulaciji. Zato mora biti
			// velikost populacije veckratnik st. 4.
			subPop[0] = populateion[i];
			subPop[1] = populateion[i + 1];
			subPop[2] = populateion[i + 2];
			subPop[3] = populateion[i + 3];

			// Izberemo starsa oz. kandidata za crossover.
			int parent1 = TournamentSelection(subPop, 2);
			int parent2 = TournamentSelection(subPop, 2);

			// QuickFix: Dodan mehanizem za zagotavljanje dveh
			// strogo razlicnih starsev, da preprecimo prevzema
			// populacije s strani enega agenta.
			if(parent1 == parent2)
			{
				for(int j = 0; j < 100; j++)
				{
					parent2 = TournamentSelection(subPop, 2);

					if(parent1 != parent2)
					{
						break;
					}
				}
			}

			// Izvedemo crossover.
			// Player[] chilren = Crossover(subPop[parent1], subPop[parent2]);
			Player[] chilren = CrossoverWithoutHead(subPop[parent1], subPop[parent2]);

			// Otroka zmagovalcev nadomestita porazenca.
			subPop[0] = subPop[parent1];
			subPop[1] = subPop[parent2];
			subPop[2] = chilren[0];
			subPop[3] = chilren[1];

			// Izvedemo mutacijo nad novim delom populacije.
			for(int j = 0; j < subPop.Length; j++)
			{
				Mutation(subPop[j]);
			}

			nextGen[i] = subPop[0];
			nextGen[i + 1] = subPop[1];
			nextGen[i + 2] = subPop[2];
			nextGen[i + 3] = subPop[3];
		}

		return nextGen;
	}

	void IncreaseGenCount()
	{
		var textUIComp = GameObject.Find("GenerationCount").GetComponent<Text>();
		int count = int.Parse(textUIComp.text);

		count += 1;

		textUIComp.text = count.ToString();
	}

	void DisplayResult(int generation)
	{
		var textUIComp = GameObject.Find("Result").GetComponent<Text>();

		if(generation % 10 == 1)
		{
			textUIComp.text = "Result found in " + generation + "st generation.";	
		}
		else if(generation % 10 == 2)
		{
			textUIComp.text = "Result found in " + generation + "nd generation.";	
		}
		else if(generation % 10 == 3)
		{
			textUIComp.text = "Result found in " + generation + "rd generation.";	
		}
		else
		{
			textUIComp.text = "Result found in " + generation + "th generation.";
		}
	}
}
