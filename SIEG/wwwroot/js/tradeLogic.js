export default {
    methods: {
        async requestLinePayment(price, state, info) {
            try {
                let _this = this;
                payment = {
                    amount: parseInt(price),
                    currency: "TWD",
                    orderId: Date.now().toString(), //使用 Timestamp 當作 orderId
                    packages: [
                        {
                            id: "20191011I001",
                            amount: parseInt(price),
                            name: _this.productInfo.pName,
                            products: [
                                {
                                    name: _this.productInfo.pName,
                                    imageUrl: _this.productInfo.pImg,
                                    quantity: 1,
                                    price: parseInt(price),
                                }
                            ]
                        },
                    ],
                    RedirectUrls: {
                        ConfirmUrl: `${NgrokUrl}/confirm`,
                        CancelUrl: "https://c4f0-61-63-154-173.jp.ngrok.io/api/LinePay/Cancel",
                    },
                };

                window.name = JSON.stringify(info);

                // 送出交易申請至商家 server
                $.post({
                    url: baseLoginPayUrl + "Create",
                    dataType: "json",
                    contentType: "application/json",
                    data: JSON.stringify(payment),
                    success: (res) => {
                        window.location = res.info.paymentUrl.web;
                    },
                    error: (err) => {
                        console.log(err);
                    }
                })
            } catch (error) {
                console.log(error)
            }
        },
        test() {
            alert("12")
        }
    }

}