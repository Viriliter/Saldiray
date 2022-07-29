// Generated by gencpp from file supreme/reset_odometry.msg
// DO NOT EDIT!


#ifndef SUPREME_MESSAGE_RESET_ODOMETRY_H
#define SUPREME_MESSAGE_RESET_ODOMETRY_H

#include <ros/service_traits.h>


#include <supreme/reset_odometryRequest.h>
#include <supreme/reset_odometryResponse.h>


namespace supreme
{

struct reset_odometry
{

typedef reset_odometryRequest Request;
typedef reset_odometryResponse Response;
Request request;
Response response;

typedef Request RequestType;
typedef Response ResponseType;

}; // struct reset_odometry
} // namespace supreme


namespace ros
{
namespace service_traits
{


template<>
struct MD5Sum< ::supreme::reset_odometry > {
  static const char* value()
  {
    return "e1e87fc9e9e6c154eca93b9fa292cc05";
  }

  static const char* value(const ::supreme::reset_odometry&) { return value(); }
};

template<>
struct DataType< ::supreme::reset_odometry > {
  static const char* value()
  {
    return "supreme/reset_odometry";
  }

  static const char* value(const ::supreme::reset_odometry&) { return value(); }
};


// service_traits::MD5Sum< ::supreme::reset_odometryRequest> should match 
// service_traits::MD5Sum< ::supreme::reset_odometry > 
template<>
struct MD5Sum< ::supreme::reset_odometryRequest>
{
  static const char* value()
  {
    return MD5Sum< ::supreme::reset_odometry >::value();
  }
  static const char* value(const ::supreme::reset_odometryRequest&)
  {
    return value();
  }
};

// service_traits::DataType< ::supreme::reset_odometryRequest> should match 
// service_traits::DataType< ::supreme::reset_odometry > 
template<>
struct DataType< ::supreme::reset_odometryRequest>
{
  static const char* value()
  {
    return DataType< ::supreme::reset_odometry >::value();
  }
  static const char* value(const ::supreme::reset_odometryRequest&)
  {
    return value();
  }
};

// service_traits::MD5Sum< ::supreme::reset_odometryResponse> should match 
// service_traits::MD5Sum< ::supreme::reset_odometry > 
template<>
struct MD5Sum< ::supreme::reset_odometryResponse>
{
  static const char* value()
  {
    return MD5Sum< ::supreme::reset_odometry >::value();
  }
  static const char* value(const ::supreme::reset_odometryResponse&)
  {
    return value();
  }
};

// service_traits::DataType< ::supreme::reset_odometryResponse> should match 
// service_traits::DataType< ::supreme::reset_odometry > 
template<>
struct DataType< ::supreme::reset_odometryResponse>
{
  static const char* value()
  {
    return DataType< ::supreme::reset_odometry >::value();
  }
  static const char* value(const ::supreme::reset_odometryResponse&)
  {
    return value();
  }
};

} // namespace service_traits
} // namespace ros

#endif // SUPREME_MESSAGE_RESET_ODOMETRY_H
