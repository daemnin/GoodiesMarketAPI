﻿using GoodiesMarket.Components.Extensions;
using GoodiesMarket.Components.Helpers;
using GoodiesMarket.Components.Models;
using GoodiesMarket.Data.Contracts;
using GoodiesMarket.Model;
using System;
using System.Linq;

namespace GoodiesMarket.Business.Processes
{
    public class ProductProcess : ProcessBase
    {
        public ProductProcess(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Result Search(Guid userId, string productName)
        {
            var result = new Result();

            try
            {
                var user = UnitOfWork.UserRepository.Read(userId);

                var sellers = UnitOfWork.SellerRepository
                                .FindBy(s => s.Products.Any(p => p.Description.Contains(productName)) &&
                                             s.User.Latitude != null &&
                                             s.User.Longitude != null &&
                                             s.Range != null,
                                             s => s.User);

                var nearBySellers = sellers.AsParallel().Where(seller => InRange(user, seller));

                result.Response = nearBySellers.ToToken();

                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                FillErrors(ex, result);
            }

            return result;
        }

        public Result Create(Guid sellerId, string name, string description, float price, int? stock)
        {
            var result = new Result();
            try
            {
                result.Succeeded = CreateProduct(sellerId, name, description, price, stock);
            }
            catch (Exception ex)
            {
                FillErrors(ex, result);
            }
            return result;

        }

        private bool CreateProduct(Guid sellerId, string name, string description, float price, int? stock)
        {

            var product = new Product
            {
                SellerId = sellerId,
                Name = name,
                Description = description,
                Price = price,
                Stock = stock
            };

            UnitOfWork.ProductRepository.Create(product);

            return UnitOfWork.Save() > 0;
        }

        private bool InRange(User user, Seller seller)
        {
            double distance = DistanceHelper.Instance.Calculate(user.Latitude.Value, user.Longitude.Value, seller.User.Latitude.Value, seller.User.Longitude.Value);

            return distance <= seller.Range;
        }
    }
}