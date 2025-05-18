import React from "react";

function ProductList({ addToCart }) {
    const products = [
        { id: 1, name: "Tarot Deck", price: 25, image: "https://via.placeholder.com/150" },
        { id: 2, name: "Candle", price: 10, image: "https://via.placeholder.com/150" },
        { id: 3, name: "Amethyst Crystal", price: 15, image: "https://via.placeholder.com/150" },
    ];

    return (
        <div className="container mt-4">
            <div className="row">
                {products.map((product) => (
                    <div key={product.id} className="col-md-4 mb-3">
                        <div className="card">
                            <img src={product.image} className="card-img-top" alt={product.name} />
                            <div className="card-body">
                                <h5 className="card-title">{product.name}</h5>
                                <p className="card-text">${product.price}</p>
                                <button className="btn btn-primary" onClick={() => addToCart(product)}>
                                    Add to Cart
                                </button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default ProductList;