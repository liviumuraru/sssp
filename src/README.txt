Problem:
	We are building a social network.  In this social network, each user has friends.

	A chain of friends between two users, user A and user B, is a sequence of users starting with A and ending with B, such that for each user in the chain, ua, the subsequent user, ua + 1, are friends.

	Given a social network and two users, user A and user B, please write a function that computes the length of the shortest chain of friends between A and B.



How did you represent the social network?  Why did you choose this representation?
	I've chosen to represent the problem using graphs, G = (V, E), where:
		G is the set of nodes representing people in the social network
		V is the set of edges representing friendships between people, such that if v and w are friends and they are both included in G, there exists an edge vw or wv in V
	This representation is currently the best in terms of required memory space and algorithms that can be applied on it.

What algorithm did you use to compute the shortest chain of friends?  What alternatives did you consider?  Why did you choose this algorithm over the alternatives?
	This is a simplified SSSP(single source shortest path) problem in an undirected graph, in which the weight of each edge is exactly 1 and the destination is given a priori
	Since we're simulating a social network, it is safe to assume the network grows "wide", not "deep", so algorithms exploring the network using a depth approach would, in my opinion, not fit the problem as good as algorithms exploring the breadth of the network.
	So the simple approach is to consider straightforward classical algorithms for the SSSP problem(in an undirected graph); algorithms I have considered are(complexities are given based on the specific requirement of the problem):
		Djikstra (O(V^2) computational worst-case complexity which stems from non-identical edge weights compared to BFS)
		Tarjan's algorithm (O(V*logV + E))
		BFS/DFS traversals (O(E + V))
	I have also considered Thorup's O(E) algorithm, but the author's paper is currently behind a paywall and patented
	However, the problem requires to only find the length of the shortest path between two nodes that are known a priori, which means that certain optimizations on the above algorithms can be applied to minimize the worst-case memory space complexity
	Given the above, I've used a simple BFS(breadth first search) exploration of the graph due to it's practical usefulness in social network graphs.
	Improvements could be done to BFS in the case of large network graphs, such as parallelizing it.
	Other improvements that could be applied to my specific implementation are:
		- bucketing the adjacent edges of a node before starting the algorithm, such that accessing that bucket would be an operation of worst-case constant complexity O(1) - hash tables maybe

Please enumerate the test cases you considered and explain their relevance.
	- input is given as a json file(see graph1.json or graph2.json for the format)
	- see graph1.json and graph2.json; they both contain at least 1 cycle; graph2.json is composed of 2 clique graphs connected using 1 edge, and one of the cliques also contains at least one cycle
	- I have also created the ability to randomly generate such a social network, but the algorithm does not guarantee a path exists between any two nodes v and w; I have used this mostly for testing purposes