
--1.	Obtener la lista de los productos no descatalogados incluyendo el nombre de la categoría ordenado por nombre de producto.
	  select pro.[ProductID]
		  ,pro.[ProductName]
		  ,pro.[SupplierID]
		  ,pro.[CategoryID]
		  ,pro.[QuantityPerUnit]
		  ,pro.[UnitPrice]
		  ,pro.[UnitsInStock]
		  ,pro.[UnitsOnOrder]
		  ,pro.[ReorderLevel]
		  ,pro.[Discontinued]
		  ,cat.CategoryName
		  from [Northwind].[dbo].[Products] pro
	  join [Northwind].[dbo].Categories cat on pro.CategoryID = cat.CategoryID
	  where pro.Discontinued=0
	  order by pro.ProductName

  --2.	Mostrar el nombre de los clientes de Nancy Davolio.
	  -- Se entiende por cliente a CompanyName
	  -- Lo normal es hacer la búsqueda por su id de empleado
	  select cus.CompanyName FROM [Northwind].[dbo].[Orders] ord
	  join [Northwind].[dbo].[Customers] cus on ord.CustomerID = cus.CustomerID
	  where ord.EmployeeID=1
	  group by cus.CompanyName
	  order by cus.CompanyName
  
	  -- En el caso que no tengamos el id del empleado, usamos su nombre directamente
	  select cus.CompanyName FROM [Northwind].[dbo].[Orders] ord
	  join [Northwind].[dbo].[Customers] cus on ord.CustomerID = cus.CustomerID
	  join [Northwind].[dbo].[Employees] emp on ord.EmployeeID = emp.EmployeeID
	  where emp.FirstName = 'Nancy'
	  and emp.LastName='Davolio'
	  group by cus.CompanyName
	  order by cus.CompanyName

  --3.	Mostrar el total facturado por año del empleado Steven Buchanan. (5)

	  -- Lo normal es hacer la búsqueda por su id de empleado
	  SELECT 
		YEAR(OrderDate) AS Año,
		SUM((UnitPrice-Discount) * Quantity) AS 'Total facturado'
	  from [dbo].[Orders] ord
	  join [Order Details] det on ord.OrderID = det.OrderID
	  where ord.EmployeeID=5
	  group by YEAR(OrderDate)
	  order by YEAR(OrderDate)

	  -- En el caso que no tengamos el id del empleado, usamos su nombre directamente
	  SELECT 
		YEAR(OrderDate) AS Año,
		SUM((UnitPrice-Discount) * Quantity) AS 'Total facturado'
	  from [dbo].[Orders] ord
	  join [Order Details] det on ord.OrderID = det.OrderID
	  join [Employees] emp on ord.EmployeeID = emp.EmployeeID
	  where emp.FirstName='Steven' and emp.LastName='Buchanan'
	  group by YEAR(OrderDate)
	  order by YEAR(OrderDate)



  --4.	Mostrar el nombre de los empleados que dependan directa o indirectamente de Andrew Fuller. (2)

		-- Lo normal es hacer la búsqueda por su id de empleado
		with  Employees_CTE as 
			(
				select
					EmployeeID,
					ReportsTo,
					FirstName,
					LastName
				from   Employees
				where  EmployeeID=2
				union all
				select  child.EmployeeID
				,       child.ReportsTo
				,       child.FirstName
				,		child.LastName
				from    Employees child
				join    Employees_CTE parent
				on      child.ReportsTo= parent.EmployeeID
				)
		select  Employees_CTE.FirstName + ' ' + Employees_CTE.LastName as FullName
		from    Employees_CTE
		where Employees_CTE.EmployeeID !=2

		-- En el caso que no tengamos el id del empleado, usamos su nombre directamente
		with  Employees_CTE as 
				(
				select EmployeeID,
					ReportsTo,
					FirstName,
					LastName
				from   Employees
				where   FirstName = 'Andrew' and LastName='Fuller'
				union all
				select  child.EmployeeID
				,       child.ReportsTo
				,       child.FirstName
				,		child.LastName
				from    Employees child
				join    Employees_CTE parent
				on      child.ReportsTo= parent.EmployeeID
				)
		select  Employees_CTE.FirstName + ' ' + Employees_CTE.LastName as FullName
		from    Employees_CTE
		where Employees_CTE.FirstName != 'Andrew' and Employees_CTE.LastName != 'Fuller'