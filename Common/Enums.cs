using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetsApi.Common
{
    public class Enums
    {
        public enum PurchaseOrderStatus
        {
            Opened = 1,
            Cancelled = 2
        }

        public enum UserRole
        {
            System = 0,
            Admin = 1,
            Authorizer = 2,
            Supplier = 3,
            Client = 4
        }

        public enum SupplierStatus {
            // Cuando ingreso al portal y aun no he sido autorizado.
            Pending = 1,
            DocumentsRequested = 2,
            // Una vez el autorizador o adminitrador cambian el estado a este.
            Authorized = 3,
            // No me permite ingresar al portal.
            Cancelled = 4
        }

        public enum NotificationType {
            CarrierRegistered = 1,
            CarrierApprovalRequest = 2,
            CarrierApprovalInformation = 3,
            CarrierApprovalApproved = 4,
            CarrierApprovalRejected = 5,
            QuoteCreated = 6,
            QuoteCounteroffer = 7,
            QuoteAccepted = 8,
            QuoteRejected = 9,
            TransportCancelApprovalRequest = 10
        }

        public enum ShipmentStatus {
            // Cargada en el load list
            NotAssigned = 1,
            // En aprobación por el manager
            PendingForRateApproval = 10,
            // Cotizar o en subasta
            Quote = 20,
            /// Cuando se le asigna a un transportista. 
            Assigned = 30,
            // Cuando el trasportista actualiza que recogio el material (uno a uno).
            InTransit = 40,
            // En aprobación por el manager para cancelar el transporte
            PendingForCancelApproval = 50,
            // :o actualiza el trasportista al subir los documentos de entrega o actualizado manualmente.
            Delivered = 60,
            // una vez se suba la factura. 
            AP = 70,
            // Cuando se cancela la carga
            LoadCancelled = 80,
            // Cuando se cancela el transportista
            TransportCancelled = 90
        }
    }
}
