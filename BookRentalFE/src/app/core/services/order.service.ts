import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../models/delivery-method.model';
import { CreateOrderRequest, Order, OrderWithDetails } from '../models/order.model';
import { Pagination } from '../models/pagination.model';
import { DeliveryMethodParams, OrderParams } from '../models/params.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  order?: Order
  ordersList: Order[] = []
  orders: Order[] = []
  orderPagination?: Pagination<Order[]>
  adminOrderPagination?: Pagination<Order[]>
  orderParams = new OrderParams()
  adminOrderParams = new OrderParams()
  orderCache = new Map<string, Pagination<Order[]>>()
  adminOrderCache = new Map<string, Pagination<Order[]>>()
  deliveryMethods: DeliveryMethod[] = []
  dmList: DeliveryMethod[] = []
  adminDmPagination?: Pagination<DeliveryMethod[]>
  adminDmParams = new DeliveryMethodParams()
  adminDmCache = new Map<string, Pagination<DeliveryMethod[]>>()

  constructor(private http: HttpClient) { }

  setOrderParams(params: OrderParams) {
    this.orderParams = params
  }

  getOrderParams() {
    return this.orderParams
  }

  setAdminOrderParams(params: OrderParams) {
    this.adminOrderParams = params
  }

  getAdminOrderParams() {
    return this.adminOrderParams
  }

  setAdminDmParams(params: DeliveryMethodParams) {
    this.adminDmParams = params
  }

  getAdminDmParams() {
    return this.adminDmParams
  }

  getUserOrders(useCache = true): Observable<Pagination<Order[]>> {
    if (!useCache) {
      this.orderCache = new Map()
    }
    if (this.orderCache.size > 0 && useCache) {
      if (this.orderCache.has(Object.values(this.orderParams).join('-'))) {
        this.orderPagination = this.orderCache.get(Object.values(this.orderParams).join('-'))

        if(this.orderPagination) {
          return of(this.orderPagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.orderParams.sort)
      params = params.append('sort', this.orderParams.sort)
    if (this.orderParams.orderStatus)
      params = params.append('orderStatus', this.orderParams.orderStatus)
    params = params.append('pageIndex', this.orderParams.pageIndex)
    params = params.append('pageSize', this.orderParams.pageSize)
    return this.http.get<Pagination<Order[]>>(`${environment.baseAPIUrl}/api/Orders`, { params }).pipe(
      map(response => {
        this.orders = [...this.orders, ...response.data]
        this.orderPagination = response
        return response
      })
    )
  }

  getUserOrdersForAdmin(useCache = true): Observable<Pagination<Order[]>> {
    if (!useCache) {
      this.adminOrderCache = new Map()
    }
    if (this.adminOrderCache.size > 0 && useCache) {
      if (this.adminOrderCache.has(Object.values(this.adminOrderParams).join('-'))) {
        this.adminOrderPagination = this.adminOrderCache.get(Object.values(this.adminOrderParams).join('-'))

        if(this.adminOrderPagination) {
          return of(this.adminOrderPagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.adminOrderParams.orderEmail)
      params = params.append('orderEmail', this.adminOrderParams.orderEmail)
    if (this.adminOrderParams.startDate)
      params = params.append('startDate', new Date(this.adminOrderParams.startDate).toDateString())
    if (this.adminOrderParams.endDate)
      params = params.append('endDate', new Date(this.adminOrderParams.endDate).toDateString())
    if (this.adminOrderParams.orderStatus)
      params = params.append('orderStatus', this.adminOrderParams.orderStatus)
    params = params.append('pageIndex', this.adminOrderParams.pageIndex)
    params = params.append('pageSize', this.adminOrderParams.pageSize)
    return this.http.get<Pagination<Order[]>>(`${environment.baseAPIUrl}/api/Orders/admin/orders-list`, { params }).pipe(
      map(response => {
        this.ordersList = [...this.ordersList, ...response.data]
        this.adminOrderPagination = response
        return response
      })
    )
  }

  getOrderDetails(id: number): Observable<OrderWithDetails> {
    return this.http.get<OrderWithDetails>(`${environment.baseAPIUrl}/api/Orders/${id}`)
  }

  getAllDeliveryMethods() {
    if (this.deliveryMethods?.length > 0) {
      return of(this.deliveryMethods)
    }
    return this.http.get<DeliveryMethod[]>(`${environment.baseAPIUrl}/api/Orders/delivery-methods`).pipe(
      map(response => {
        this.deliveryMethods = response.sort((a, b) => b.price - a.price)
        return response
      })
    )
  }

  getAllDeliveryMethodsForAdmin(useCache = true): Observable<Pagination<DeliveryMethod[]>> {
    if (!useCache) {
      this.adminDmCache = new Map()
    }
    if (this.adminDmCache.size > 0 && useCache) {
      if (this.adminDmCache.has(Object.values(this.adminDmParams).join('-'))) {
        this.adminDmPagination = this.adminDmCache.get(Object.values(this.adminDmParams).join('-'))
        if(this.adminDmPagination) {
          return of(this.adminDmPagination)
        }
      }
    }
    let params = new HttpParams()
    params = params.append('pageIndex', this.adminDmParams.pageIndex)
    params = params.append('pageSize', this.adminDmParams.pageSize)
    if (this.adminDmParams.search) params = params.append('search', this.adminDmParams.search)
    return this.http.get<Pagination<DeliveryMethod[]>>(`${environment.baseAPIUrl}/api/Orders/admin/delivery-methods-list`, { params }).pipe(
      map(response => {
        this.dmList = [...this.dmList, ...response.data]
        this.adminDmPagination = response
        return response
      })
    )
  }

  importDmsFromFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/Orders/delivery-method/import-from-file`, formData).pipe(
      tap(() => {
        this.adminDmCache = new Map()
      })
    )
  }

  deleteDm(id: number) {
    return this.http.delete(`${environment.baseAPIUrl}/api/Orders/delivery-method/soft-delete/${id}`)
  }

  createOrder(request: CreateOrderRequest, basketId: string): Observable<any> {
    return this.http.post(`${environment.baseAPIUrl}/api/Orders/${basketId}`, request)
  }

  acceptOrder(orderId: number, userEmail: string): Observable<any> {
    return this.http.put(`${environment.baseAPIUrl}/api/Orders/accept/${userEmail}/${orderId}`, {}).pipe(
      tap(() => {
        this.adminOrderCache = new Map()
        this.orderCache = new Map()
      })
    )
  }

  declineOrder(orderId: number, userEmail: string): Observable<any> {
    return this.http.put(`${environment.baseAPIUrl}/api/Orders/decline/${userEmail}/${orderId}`, {}).pipe(
      tap(() => {
        this.adminOrderCache = new Map()
        this.orderCache = new Map()
      })
    )
  }
}
