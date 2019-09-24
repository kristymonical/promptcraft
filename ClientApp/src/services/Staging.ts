export namespace Services {
  export namespace Staging {
    export interface CreateStagingRequest {
      cartIds: string[];
      destinationArea: string;
      requestType: string;
    }
    export async function createStagingRequest({
      cartIds,
      destinationArea,
      requestType
    }: CreateStagingRequest) {}

    export async function getStagedCarts() {}
  }
}
