# Основные структуры

## Представление графов
Графы представлены в виде трех структур (классов):
- `GraphMaster`
    - oriented (bool)
    - weighted (bool)
    - negativeEdges (bool)
    - hasLoops (bool)
    - graphType (`GraphType`)
    - graphNodes (List(`GraphNode`))
    - func MakeDijkstra() => List(`GraphNode`)
    - func MakeBreadth() => List(`GraphNode`)
    - func MakeDepth() => List(`GraphNode`)
    - func MakeBF() => List(`GraphNode`)
- `GraphNode`
    - number (int id)
    - description (string)
    - name (string)
    - edges (List(`GraphEdge`))
- `GraphEdge`
    - node (`GraphNode`)
    - weight (float)
- `EdgesFreedom (enum)`
    - DERECTED
    - UNDIRECTED


При этом если граф направленный, то GraphEdge.node указывает вершину в которую направлено данное ребро.
Соответственно если граф ненаправленный то для каждых двух вершин между которыми есть путь будет два ребра. 

## Алгоритмы
