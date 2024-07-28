using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Compañias
{
    class CompanyService
    {
        public ObservableCollection<Company> Companies { get; }
        public CompanyService() => Companies = new ObservableCollection<Company>();

        public void AddCompany(Company c)
        {
            if (Companies.Contains(c))
                throw new CompanyException($"{c.Name} ya se encuentra en la lista.");
            Companies.Add(c);
        }

        public void UpdateCompanyData(Company c)
        {
            if (!Companies.Contains(c))
                throw new CompanyException($"{c.Name} no se encuentra en la lista para poder ser actualizada");

            var existingCompany = Companies.FirstOrDefault(comp => comp.Equals(c));
            if (existingCompany != null)
            {
                existingCompany.Telephone = c.Telephone;
                existingCompany.Email = c.Email;
            }
        }

        public void RemoveCompanyData(Company c)
        {
            if (!Companies.Contains(c))
                throw new CompanyException($"{c.Name} no se encuentra en la lista para poder ser eliminada");

            Companies.Remove(c);
        }

        public Company SeeCompanyData(Company c)
        {
            if (!Companies.Contains(c))
            {
                throw new CompanyException($"{c.Name} no se encuentra en la lista.");
            }

            return Companies.FirstOrDefault(comp => comp.Equals(c));
        }
    }
}
