using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieTheaterWS_v2.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class TopRatingController : ControllerBase
    {
        //[HttpPost]
        public List<int> ListaSinDuplicados([FromBody] List<int> lista)
        {
            List<int> resultado = new List<int>();
            resultado.Add(lista[0]);

            // recorrer la lista
            for (int i = 0; i < lista.Count; i++)
            {
                int j = 0;
                var encontrado = false;
                //recorrer resultado hasta encontrar igual o hasta final
                while (j < resultado.Count && !encontrado)
                {
                    if (lista[i] == resultado[j])
                    {
                        encontrado = true;
                        // break;
                    }
                    j++;
                }
                //si no fue encontrado adicionarlo
                if(!encontrado)
                {
                    resultado.Add(lista[i]);
                }
            }

            return resultado;
        }

        //[HttpPost]
        public int GetTopProductId (int arraySize, int[][] productRatingList)
        {
            int topRatedProductId;

            List<int> listWithoutDuplicates = new List<int>();
            listWithoutDuplicates.Add(productRatingList[0][1]);

            // Loop through product rating list to fill new list without duplicates
            for (int i = 0; i < productRatingList.Length; i++)
            {
                int j = 0;
                var encontrado = false;
                // Loop listWithoutDuplicates until match is found or untill end of listWithoutDuplicates
                while (j < listWithoutDuplicates.Count && !encontrado)
                {
                    if (productRatingList[i][0] == listWithoutDuplicates[j])
                    {
                        encontrado = true;
                        // break;
                    }
                    j++;
                }
                // if it was not found add it to the new list
                if (!encontrado)
                {
                    listWithoutDuplicates.Add(productRatingList[i][0]);
                }
            }

            List<int> numberOfRatingsPerProduct = new List<int>();
            List<int> ratingSumPerProduct = new List<int>();

            // Check productRatingList to see how many times appear each product and sum ratings per product
            for(int i = 0;i < productRatingList.Length;i++)
            {
                for (int j = 0;j < listWithoutDuplicates.Count;j++)
                {
                    if (productRatingList[i][0] == listWithoutDuplicates[j])
                    {
                        numberOfRatingsPerProduct[j]++;
                        ratingSumPerProduct[j] += productRatingList[i][1];
                    }
                }
            }

            List<float> ratingAveragePerProduct = new List<float>();

            // Calculate average rating per product
            for (int i = 0;i < listWithoutDuplicates.Count; i++)
            {
                ratingAveragePerProduct[i] = ratingSumPerProduct[i] / numberOfRatingsPerProduct[i];
            }

            float topAverage = 0;
            // Loop through ratingAveragePerProduct to get top average rating
            for (int i = 0; i < ratingAveragePerProduct.Count; i++)
            {
                if (ratingAveragePerProduct[i] > topAverage)
                {
                    topAverage = ratingAveragePerProduct[i];
                }
            }

            topRatedProductId = listWithoutDuplicates[0];
            // Loop through ratingAveragePerProduct, if there is more than one product with equal top rating set product with lowest id
            for (int i = 0; i< ratingAveragePerProduct.Count; i++)
            {
                if (ratingAveragePerProduct[i] == topAverage )
                {
                    if(listWithoutDuplicates[i] < topRatedProductId)
                    {
                        topRatedProductId = listWithoutDuplicates[i];
                    }
                }
            }

            return topRatedProductId;

        }

        // GET: TopRatingController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: TopRatingController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: TopRatingController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: TopRatingController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: TopRatingController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: TopRatingController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: TopRatingController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: TopRatingController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
