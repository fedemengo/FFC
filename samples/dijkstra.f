/*
    O(V^2) implementation of Dijkstra's algorithm for a weighted graph given as an adjacency list,
    represented as array of array of tuple(int, int)
    Using map for 
*/

dijkstra is func(adj :[[(integer, integer)]], source :integer) :[integer] do
    
    inf is 2000000000;
    
    //distance array
    dist :[integer] is [];
    for i in 1 ..length(adj) loop
        dist := dist + inf;
    end
    dist[source] := source;

    //queue declaration
    queue :[integer] is [];
    for i in 0 ..(length(adj)-1) loop
        queue := queue + dist[i];
    end

    get_min is func(v: [integer], ignore: integer) :integer do
        ans is -1;
        for i in 0 ..length(size) loop
            if v[i] = skip then
                continue;
            end
            if ans = -1 | v[i] < v[ans] then
                ans := i;
            end
        end
        return ans;
    end

    b is get_min(queue, inf);
    while b /= -1 loop
        //relax paths
        for e in adj[b] loop
            //skip already used nodes
            if queue[e.1] = inf then
                continue;
            end
            //if we can improve minimum distance
            dist[e.1] := min(dist[e.1], dist[b] + e.2);
            queue[e.1] := dist[e.1];
        end
        //remove b from the queue
        queue[b] := inf;
    end

    return dist;

end
