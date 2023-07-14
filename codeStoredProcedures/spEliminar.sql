CREATE PROCEDURE spEliminar
   @idExamen int
AS
BEGIN
   SET NOCOUNT ON;

   DECLARE @ErrorMessage NVARCHAR(4000);
   DECLARE @ErrorState INT;

   IF @idExamen IS NULL OR NOT EXISTS (SELECT 1 FROM tblExamen WHERE idExamen = @idExamen)
   BEGIN
      SET @ErrorState = 1;
      SET @ErrorMessage = 'El registro con el idExamen proporcionado no se encontr� o es nulo.';
      SELECT @ErrorState AS ReturnCode, @ErrorMessage AS Description;
      RETURN;
   END

   BEGIN TRY
      -- Elimina el registro en la tabla tblExamen basado en el id
      DELETE FROM tblExamen
      WHERE idExamen = @idExamen;

      -- Regresa el c�digo de �xito y la descripci�n
      SELECT 0 AS ReturnCode, 'Registro eliminado satisfactoriamente' AS Description;
   END TRY
   BEGIN CATCH
      -- Recopila los detalles del error
      SET @ErrorMessage = ERROR_MESSAGE();
      SET @ErrorState = ERROR_STATE();

      -- Regresa el c�digo del error y la descripci�n
      SELECT @ErrorState AS ReturnCode, @ErrorMessage AS Description;
   END CATCH;
END