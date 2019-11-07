interface ServiceResponse<TData> {
  success: boolean;
  message?: string;
  data: TData;
}

export default ServiceResponse;
