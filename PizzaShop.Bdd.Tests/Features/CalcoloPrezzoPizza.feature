Feature: Calcolo del prezzo di una pizza
  Come cliente della pizzeria
  Voglio conoscere il prezzo di una pizza in base al formato e agli ingredienti extra
  Così da sapere quanto pagherò prima di ordinare

  Background:
	Given che sono configurati i seguenti prezzi base
	  | Formato | PrezzoBase |
	  | Small   | 5.00       |
	  | Medium  | 7.50       |
	  | Large   | 10.00      |
	And che sono disponibili i seguenti topping
	  | Nome       | Prezzo |
	  | Mozzarella | 1.00   |
	  | Funghi     | 1.20   |
	  | Pepperoni  | 1.50   |
	  | Prosciutto | 1.30   |
	  | Olive      | 0.80   |

  Scenario Outline: Prezzo calcolato in base a formato e ingredienti extra
	Given che ordino una pizza di formato "<Formato>"
	When aggiungo i seguenti ingredienti extra "<Ingredienti>"
	Then il prezzo della pizza dovrebbe essere "<PrezzoAtteso>" euro

	Examples:
	  | Formato | Ingredienti                 | PrezzoAtteso |
	  | Small   |                             | 5.00         |
	  | Medium  | Mozzarella                  | 8.50         |
	  | Large   | Mozzarella,Funghi           | 12.20        |
	  | Large   | Pepperoni,Prosciutto,Olive  | 13.60        |
