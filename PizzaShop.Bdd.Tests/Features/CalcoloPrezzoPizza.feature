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

    Examples: Senza ingredienti extra, vale il prezzo base
      | Formato | Ingredienti | PrezzoAtteso |
      | Small   |             | 5.00         |
      | Medium  |             | 7.50         |
      | Large   |             | 10.00        |

    Examples: Con ingredienti extra, si somma il prezzo di ciascuno
      | Formato | Ingredienti                 | PrezzoAtteso |
      | Medium  | Mozzarella                  | 8.50         |
      | Medium  | Mozzarella,Funghi,Pepperoni | 11.20        |
      | Large   | Mozzarella,Funghi           | 12.20        |
      | Large   | Pepperoni,Prosciutto,Olive  | 13.60        |

    Examples: Con ingredienti ripetuti, ogni ripetizione viene pagata
      | Formato | Ingredienti           | PrezzoAtteso |
      | Medium  | Mozzarella,Mozzarella | 9.50         |
      | Large   | Olive,Olive,Olive     | 12.40        |
