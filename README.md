# Hashset
Unity Vizualization Project

Span of Project: 9/1-23/19

**Vizualization Background**  
Vizualise how a hashset stores its data.

**Where To View**  
https://simmer.io/@apkirito/hashset

**What To Do**
- Add nodes to the hashset
- Delete nodes from the hashset
- Choose a different collision resolution (chaining or probing)
- When Load Capacity is full, the hashset will rehash all its nodes

**Script Accomplishments**
- Used a Unity's Event system to orchestrate movements.

**Notes**
- Chaining resolution simulates a Linked List where collided nodes are linked next to each other.
- Probing resolution follows the rule of incrementing an index until an empty index is found.
- The Load Capacity is set to .75, so the Hashset will rehash when #nodes/#buckets > .75.
- The bucket size increases linearly.
