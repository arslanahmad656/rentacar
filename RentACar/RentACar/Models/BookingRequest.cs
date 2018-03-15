//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentACar.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BookingRequest()
        {
            this.Bookings = new HashSet<Booking>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> SubLocationId { get; set; }
        public System.DateTime RequestDate { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public bool WithDriver { get; set; }
        public Nullable<int> PaymentMethodId { get; set; }
        public System.DateTime RequestInitiated { get; set; }
        public Nullable<System.DateTime> RequestViewed { get; set; }
        public Nullable<int> RequestStatusId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Location Location { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual RequestStatu RequestStatu { get; set; }
        public virtual Sublocation Sublocation { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}