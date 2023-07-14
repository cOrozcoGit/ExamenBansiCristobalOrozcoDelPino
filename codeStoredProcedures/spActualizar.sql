CREATE PROCEDURE spActualizar
   @idExamen int,
   @Nombre varchar(255),
   @Descripcion varchar(255)
AS
BEGIN
   SET NOCOUNT ON;
   
   DECLARE @ErrorMessage NVARCHAR(4000);
   DECLARE @ErrorState INT;

   IF @idExamen IS NULL OR NOT EXISTS (SELECT 1 FROM tblExamen WHERE idExamen = @idExamen)
   BEGIN
      SET @ErrorState = 1;
      SET @ErrorMessage = 'el registro con el idExamen proporcionado no se encontro o es nulo.';
      SELECT @ErrorState AS ReturnCode, @ErrorMessage AS Description;
      RETURN;
   END

   BEGIN TRY
        -- Actualiza un registro en la tabla tblExamen basado en el id
      UPDATE tblExamen 
      SET Nombre = @Nombre, Descripcion = @Descripcion
      WHERE idExamen = @idExamen;

        -- Regresa el codigo de exito y la descripcion
       SELECT 0 AS ReturnCode, 'Registro actualizado satisfactoriamente' AS Description;
   END TRY
   BEGIN CATCH
       -- Recopila los detalles del error
       SET @ErrorMessage = ERROR_MESSAGE();
       SET @ErrorState = ERROR_STATE();

       -- Regrese el codigo del error y la descripcion
       SELECT @ErrorState AS ReturnCode, @ErrorMessage AS Description;
   END CATCH;
END
