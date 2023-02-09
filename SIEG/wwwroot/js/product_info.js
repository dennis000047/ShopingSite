export default {
    methods: {
        async getProductInfoDTO(pID) {
            try {
                let response = await axios.get(`${webApiAddress}/api/ProductInfo/${pID}`);
                return response.data;
            } catch (error) {
                console.log(error);
            }
        }
    }
}