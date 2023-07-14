CREATE PROCEDURE spAgregar
   @Nombre varchar(255),
   @Descripcion varchar(255)
AS
BEGIN
   SET NOCOUNT ON;
   
   DECLARE @ErrorMessage NVARCHAR(4000);
   DECLARE @ErrorState INT;

   BEGIN TRY
        -- Inserta una nuevo registro en la tabla tblExamen
     INSERT INTO tblExamen (Nombre, Descripcion)
     VALUES (@Nombre, @Descripcion);

        -- Regresa el codigo de exito y la descripcion
      SELECT 0 AS ReturnCode, 'Registro insertado satisfactoriamente' AS Description;
   END TRY
   BEGIN CATCH
      -- Recopila los detalles del error
      SET @ErrorMessage = ERROR_MESSAGE();
      SET @ErrorState = ERROR_STATE();

      -- Regrese el codigo del error y la descripcion
      SELECT @ErrorState AS ReturnCode, @ErrorMessage AS Description;
   END CATCH;
END
