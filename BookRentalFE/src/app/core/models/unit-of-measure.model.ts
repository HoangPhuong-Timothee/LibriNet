export interface UnitOfMeasure {
  id: number
  name: string
  description?: string
  createInfo?: string
  updateInfo?: string
}

export interface AddUnitOfMeasureRequest {
  name: string
  description: string
  conversionRate: number
  mappingUnitId: number
}

export interface UpdateUnitOfMeasureRequest {
  id: number
  name: string
  description: string
  conversionRate: number
  mappingUnitId: number
}
