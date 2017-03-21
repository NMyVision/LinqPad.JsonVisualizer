# NMyVision.LinqPad.JsonVisualizer
Json Visualizer for LINQPad

Dump results to a JSON tree with collapse and expand features


## Viewing JSON
To view JSON results plan or missing indexes call static `QueryPlanVisualizer.JsonVisualizer.DumpPlan(query)` method or call `DumpJSON` extension method on any object instance. You will also need to add `NMyVision.JsonVisualizer` to namespaces list (click F4 to open the dialog). If you want to name the window pass a title as a second parameter.

Json visualizer:
![missing indexes](screenshots/linqpadvisualizer.gif "Json Visualizer")
