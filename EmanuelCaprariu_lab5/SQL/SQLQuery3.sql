SELECT [o].[OrderLineId], [p].[RegistrationCode], [p].[Description], [o].[Amount], [o0].[Address], [o].[Price], [o0].[Total]
FROM [OrderLine] AS [o]
INNER JOIN [Product] AS [p] ON [o].ProductId = [p].[ProductId]
INNER JOIN [OrderHeader] AS [o0] ON [o].[OrderId] = [o0].[OrderId]


SELECT * FROM OrderLine
INNER JOIN Product on OrderLine.ProductId = Product.ProductId
INNER JOIN OrderHeader on OrderHeader.OrderId = OrderLine.OrderId