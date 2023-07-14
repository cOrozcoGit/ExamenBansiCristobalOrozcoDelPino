CREATE PROCEDURE spConsultar
   @Nombre varchar(255),
   @Descripcion varchar(255)
AS
BEGIN
	SELECT * FROM tblExamen WHERE Nombre = @Nombre AND Descripcion = @Descripcion;
END