<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupduserinfo">
    <table class="tblDesc">
      <tr>
        <td class="colW1">{lbl:fname}</td>
        <td class="colW2">
          <xsl:value-of select="@fname" />
        </td>
        <td class="colW1">{lbl:lname}</td>
        <td class="colW2">
          <xsl:value-of select="@lname" />
        </td>

        <td class="colW1">{lbl:SubAccName}</td>
        <td class="colW2">

          <xsl:value-of select="@subaccname" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:address}</td>
        <td class="colW2">
          <xsl:value-of select="@address" />
        </td>
        
        <td class="colW1">{lbl:postal}</td>
        <td class="colW2">
          <xsl:value-of select="@postal" />
        </td>
        <td class="colW1">{lbl:city}</td>
        <td class="colW2">
          <xsl:value-of select="@city" />
        </td>

      </tr>
      <tr>
        <td class="colW1">{lbl:state}</td>
        <td class="colW2">
          <xsl:value-of select="@state" />
        </td>
        <td class="colW1">{lbl:country}</td>
        <td class="colW2">
          <xsl:value-of select="@country" />
        </td>
        <td class="colW1">{lbl:phone}</td>
        <td class="colW2">
          <xsl:value-of select="@phone" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:mphone}</td>
        <td class="colW2">
          <xsl:value-of select="@mphone" />
        </td>
        
        <td class="colW1">{lbl:fax}</td>
        <td class="colW2">
          <xsl:value-of select="@fax" />
        </td>
        <td class="colW1">{lbl:email}</td>
        <td class="colW2">
          <xsl:value-of select="@email" />
        </td>

        
      </tr>
      <tr>
        <td class="colW1">{lbl:birthday}</td>
        <td class="colW2">
          <xsl:value-of select="substring(@birthday, 1, 12)" />
          <!--<xsl:value-of select="@birthday" />-->
        </td>
        <td class="colW1">{lbl:refcode1}</td>
        <td class="colW2">
          <xsl:value-of select="@refcode1" />
        </td>
        <td class="colW1">{lbl:refcode2}</td>
        <td class="colW2">
          <xsl:value-of select="@refcode2" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:remark}</td>
        <td class="colW2">
          <xsl:value-of select="@remark" />
        </td>
        <td class="colW1">{lbl:gender}</td>
        <td class="colW2">
          <xsl:value-of select="@gender" />
        </td>
        <td class="colW1">{lbl:Language}</td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:IP}</td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        <td class="colW1">{lbl:ErrCode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>

        <td class="colW1">{lbl:ErrMsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        
      </tr>


      <xsl:choose>
        <xsl:when test="$IsInternal = 0">

        </xsl:when>
        <xsl:otherwise>
          <tr>
            <xsl:choose>
              <xsl:when test="@subaccname">
                <td class="colW1">{lbl:subaccname}</td>
                <td class="colW2">
                  <xsl:value-of select="@subaccname" />
                </td>
              </xsl:when>
              <xsl:otherwise>
                <td class="colW1">{lbl:subAccID}</td>
                <td class="colW2">
                  <xsl:value-of select="@subaccid" />
                </td>
              </xsl:otherwise>
            </xsl:choose>
            <td class="colW1">{lbl:important}</td>
            <td class="colW2">
              <xsl:value-of select="@important" />
            </td>

            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>
        

    </table>
  </xsl:template>

</xsl:stylesheet>
