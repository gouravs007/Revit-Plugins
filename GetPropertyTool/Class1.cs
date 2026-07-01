using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
//using System;
//using System.Reflection.Metadata;
//using System.Xml.Linq;

namespace MepAutomationTools
{
    [Transaction(TransactionMode.Manual)]
    public class IsolateMepSystem : IExternalCommand
    {
        public Result Execute(
         ExternalCommandData commandData,
         ref string message,
         ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;


            Autodesk.Revit.DB.Document doc = uiDoc.Document;

            try
            {
                Reference pickedRef = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select an MEP element to inspect its system:");
                Autodesk.Revit.DB.Element selectedElement = doc.GetElement(pickedRef);

                string systemName = "None / Unassigned";

                Autodesk.Revit.DB.Parameter sysParam = selectedElement.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM);
                if (sysParam != null && !string.IsNullOrEmpty(sysParam.AsString()))
                {
                    systemName = sysParam.AsString();
                }

                TaskDialog.Show("MEP Data Engine",
                    $"Element ID: {selectedElement.Id}\n" +
                    $"Category: {selectedElement.Category.Name}\n" +
                    $"Assigned Network System: {systemName}");

                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}